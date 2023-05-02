using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TatBlog.Core.DTO;
using TggWeb.Core.Contracts;
using TggWeb.Core.DTO;
using TggWeb.Core.Entities;
using TggWeb.Data.Contexts;
using TggWeb.Services.Extensions;

namespace TggWeb.Services.Web
{
	public class WebRepository : IWebRepository
	{
		private readonly WebDbContext _context;
		private readonly IMemoryCache _memoryCache;

		public WebRepository(WebDbContext context, IMemoryCache memoryCache)
		{
			_context = context;
			_memoryCache = memoryCache;
		}

		// Lấy 1 bài viết theo id
		public async Task<Post> GetPostByIdAsync(
		int postId, bool includeDetails = false,
		CancellationToken cancellationToken = default)
		{
			if (!includeDetails)
			{
				return await _context.Set<Post>().FindAsync(postId);
			}

			return await _context.Set<Post>()
				.Include(x => x.Game)
				.Include(x => x.Game.Category)
				.Include(x => x.Tags)
				.FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
		}

		// Lấy 1 bài viết theo tên định danh
		public async Task<Post> GetPostAsync(
		string slug,
		CancellationToken cancellationToken = default)
		{
			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				TitleSlug = slug
			};

			return await FilterPosts(postQuery).FirstOrDefaultAsync(cancellationToken);
		}

		// Lấy thông tin chi tiết của 1 bài viết
		public async Task<Post> GetPostDetailAsync(
		PostQuery condition,
		CancellationToken cancellationToken = default)
		{
			return await FilterPosts(condition).FirstOrDefaultAsync(cancellationToken);
		}

		// Tìm và phân trang các bài viết theo điều kiện
		public async Task<IPagedList<Post>> GetPagedPostsAsync(
		PostQuery condition,
		int pageNumber = 1,
		int pageSize = 10,
		CancellationToken cancellationToken = default)
		{
			return await FilterPosts(condition).ToPagedListAsync(
				pageNumber, pageSize,
				nameof(Post.PostedDate), "DESC",
				cancellationToken);
		}

		// Tìm N bài viết phổ biến nhất
		public async Task<IList<Post>> GetPopularArticlesAsync(
			int numPosts, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Post>()
				.Include(x => x.Game)
				.Include(x => x.Game.Category)
				.Include(x => x.Tags)
				.OrderByDescending(p => p.ViewCount)
				.Take(numPosts)
				.ToListAsync(cancellationToken);
		}

		// Lấy ngẫu nhiên N bài viết
		public async Task<IList<Post>> GetRandomArticlesAsync(
		int numPosts, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Post>()
				.OrderBy(x => Guid.NewGuid())
				.Take(numPosts)
				.ToListAsync(cancellationToken);
		}

		// Lấy các bài viết liên quan
		public async Task<IList<Post>> GetRelatedPostsAsync(int postId, int categoryId)
		{
			var relatedPosts = await _context.Posts
				.Where(p => p.Id != postId && p.Game.Category.Id == categoryId && p.Published)
				.OrderByDescending(p => p.PostedDate)
				.Take(5)
				.ToListAsync();

			return relatedPosts;
		}

		// Lấy bài viết theo tháng
		public async Task<IList<Post>> GetPostsByMonthAsync(int year, int month)
		{
			var posts = await _context.Posts
				.Where(p => p.PostedDate.Year == year && p.PostedDate.Month == month)
				.ToListAsync();
			return posts;
		}

		// Tăng số lượt xem của một bài viết
		public async Task IncreaseViewCountAsync(
			int postId, CancellationToken cancellationToken = default)
		{
			await _context.Set<Post>()
				.Where(x => x.Id == postId)
				.ExecuteUpdateAsync(p =>
				p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
				cancellationToken);
		}

		// Xóa một bài viết theo id
		public async Task<bool> DeletePostAsync(
		int postId, CancellationToken cancellationToken = default)
		{
			return await _context.Posts
				.Where(x => x.Id == postId)
				.ExecuteDeleteAsync(cancellationToken) > 0;
		}

		// Lấy thẻ
		public async Task<Tag> GetTagAsync(
		string slug, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Tag>()
				.FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
		}

		// Thêm hoặc cập nhật một bài viết.
		public async Task<Post> CreateOrUpdatePostAsync(
		Post post, IEnumerable<string> tags,
		CancellationToken cancellationToken = default)
		{
			if (post.Id > 0)
			{
				await _context.Entry(post)
							  .Collection(x => x.Tags)
							  .LoadAsync(cancellationToken);
			}
			else
			{
				post.Tags = new List<Tag>();
			}

			var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
				.Select(x => new
				{
					Name = x,
					Slug = GenerateSlug(x)
				})
				.GroupBy(x => x.Slug)
				.ToDictionary(g => g.Key, g => g.First().Name);

			foreach (var kv in validTags)
			{
				if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key,
					StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

				var tag = await GetTagAsync(kv.Key, cancellationToken) ?? new Tag()
				{
					Name = kv.Value,
					Description = kv.Value,
					UrlSlug = kv.Key
				};

				post.Tags.Add(tag);
			}

			post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

			if (post.Id > 0)
				_context.Update(post);
			else
				_context.Add(post);

			await _context.SaveChangesAsync(cancellationToken);

			return post;
		}
		public async Task<bool> AddOrUpdateAsync(
			Post post,
			IEnumerable<string> tags,
			CancellationToken cancellationToken = default)
		{
			if (post.Id > 0)
			{
				// Xóa các tag của bài viết cũ trong database
				var oldPost = await _context.Posts.Include(p => p.Tags)
					.FirstOrDefaultAsync(p => p.Id == post.Id, cancellationToken);
				oldPost.Tags.Clear();
				await _context.SaveChangesAsync(cancellationToken);

				// Thêm các tag mới cho bài viết
				foreach (var tagName in tags)
				{
					var tag = await _context.Tags
						.FirstOrDefaultAsync(t => t.Name == tagName, cancellationToken);
					if (tag == null)
					{
						tag = new Tag { Name = tagName };
						_context.Tags.Add(tag);
					}

					post.Tags.Add(tag);
				}

				_context.Posts.Update(post);
				_memoryCache.Remove($"post.by-id.{post.Id}");
			}
			else
			{
				// Thêm các tag mới cho bài viết
				foreach (var tagName in tags)
				{
					var tag = await _context.Tags
						.FirstOrDefaultAsync(t => t.Name == tagName, cancellationToken);
					if (tag == null)
					{
						tag = new Tag { Name = tagName };
						_context.Tags.Add(tag);
					}

					post.Tags.Add(tag);
				}

				_context.Posts.Add(post);
			}

			return await _context.SaveChangesAsync(cancellationToken) > 0;
		}

		// Kiểm tra xem tên định danh của bài viết đã có hay chưa 
		public async Task<bool> IsPostSlugExistedAsync(
			int postId, string slug,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Post>()
				.AnyAsync(x => x.Id != postId && x.UrlSlug == slug,
				cancellationToken);
		}

		// Tạo tên định danh (slug)
		private static string GenerateSlug(string phrase)
		{
			var str = phrase.ToLowerInvariant().Trim();
			str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
			str = Regex.Replace(str, @"\s+", " ").Trim();
			str = str.Substring(0, str.Length <= 50 ? str.Length : 50).Trim();
			str = Regex.Replace(str, @"\s", "-");

			return str;
		}

		// Đặt ảnh cho bài viết
		public async Task<bool> SetImageUrlAsync(
			int postId, string imageUrl,
		CancellationToken cancellationToken = default)
		{
			return await _context.Posts
				.Where(x => x.Id == postId)
				.ExecuteUpdateAsync(x =>
					x.SetProperty(a => a.ImageUrl, a => imageUrl),
					cancellationToken) > 0;
		}

		// Chuyển đổi trạng thái Published của bài viết. 
		public async Task<bool> TogglePublishedFlagAsync(
			int postId, CancellationToken cancellationToken = default)
		{
			var post = await _context.Set<Post>().FindAsync(postId);

			if (post is null) return false;

			post.Published = !post.Published;
			await _context.SaveChangesAsync(cancellationToken);

			return post.Published;
		}

		private IQueryable<Post> FilterPosts(PostQuery condition)
		{
			IQueryable<Post> posts = _context.Set<Post>()
			.Include(x => x.Game)
			.Include(x => x.Game.Category)
			.Include(x => x.Tags);

			if (condition.PublishedOnly)
			{
				posts = posts.Where(x => x.Published);
			}

			if (condition.NotPublished)
			{
				posts = posts.Where(x => !x.Published);
			}

			if (condition.CategoryId > 0)
			{
				posts = posts.Where(x => x.Game.Category.Id == condition.CategoryId);
			}

			if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
			{
				posts = posts.Where(x => x.Game.Category.UrlSlug == condition.CategorySlug);
			}

			if (condition.GaneId > 0)
			{
				posts = posts.Where(x => x.GameId == condition.GaneId);
			}

			if (!string.IsNullOrWhiteSpace(condition.GameSlug))
			{
				posts = posts.Where(x => x.Game.UrlSlug == condition.GameSlug);
			}

			if (!string.IsNullOrWhiteSpace(condition.TagSlug))
			{
				posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
			}

			if (!string.IsNullOrWhiteSpace(condition.Keyword))
			{
				posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
										 x.ShortDescription.Contains(condition.Keyword) ||
										 x.Description.Contains(condition.Keyword) ||
										 x.Game.Name.Contains(condition.Keyword) ||
										 x.Game.Category.Name.Contains(condition.Keyword) ||
										 x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
			}

			if (condition.Year > 0)
			{
				posts = posts.Where(x => x.PostedDate.Year == condition.Year);
			}

			if (condition.Month > 0)
			{
				posts = posts.Where(x => x.PostedDate.Month == condition.Month);
			}

			if (condition.Day > 0)
			{
				posts = posts.Where(x => x.PostedDate.Day == condition.Day);
			}

			if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
			{
				posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
			}

			return posts;
		}
	}
}
