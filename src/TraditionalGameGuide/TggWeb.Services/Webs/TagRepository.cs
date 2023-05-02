using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TggWeb.Core.Contracts;
using TggWeb.Core.DTO;
using TggWeb.Core.Entities;
using TggWeb.Data.Contexts;
using TggWeb.Services.Extensions;

namespace TggWeb.Services.Webs
{
	public class TagRepository : ITagRepository
	{
		private readonly WebDbContext _context;
		private readonly IMemoryCache _memoryCache;

		public TagRepository(WebDbContext context, IMemoryCache memoryCache)
		{
			_context = context;
			_memoryCache = memoryCache;
		}

		public async Task<Tag> GetTagBySlugAsync(
			string slug,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Tag>()
				.FirstOrDefaultAsync(t => t.UrlSlug == slug, cancellationToken);
		}

		public async Task<Tag> GetCachedTagBySlugAsync(
			string slug,
			CancellationToken cancellationToken = default)
		{
			return await _memoryCache.GetOrCreateAsync(
				$"tag.by-slug.{slug}",
				async (entry) =>
				{
					entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
					return await GetTagBySlugAsync(slug, cancellationToken);
				});
		}

		public async Task<Tag> GetTagByIdAsync(int tagId)
		{
			return await _context.Set<Tag>().FindAsync(tagId);
		}

		public async Task<Tag> GetCachedTagByIdAsync(int tagId)
		{
			return await _memoryCache.GetOrCreateAsync(
				$"tag.by-id.{tagId}",
				async (entry) =>
				{
					entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
					return await GetTagByIdAsync(tagId);
				});
		}

		public async Task<IList<TagItem>> GetTagsAsync(
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Tag>()
				.OrderBy(t => t.Name)
				.Select(t => new TagItem()
				{
					Id = t.Id,
					Name = t.Name,
					UrlSlug = t.UrlSlug,
					Description = t.Description,
					PostCount = t.Posts.Count(p => p.Published)
				})
				.ToListAsync(cancellationToken);
		}

		public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
			IPagingParams pagingParams,
			string name = null,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Tag>()
				.AsNoTracking()
				.WhereIf(!string.IsNullOrWhiteSpace(name),
					t => t.Name.Contains(name))
				.Select(t => new TagItem()
				{
					Id = t.Id,
					Name = t.Name,
					UrlSlug = t.UrlSlug,
					Description = t.Description,
					PostCount = t.Posts.Count(p => p.Published)
				})
				.ToPagedListAsync(pagingParams, cancellationToken);
		}

		public async Task<IPagedList<Tag>> GetPagedTagsQueryAsync(
			PostQuery condition,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default)
		{
			return await FilterTags(condition).ToPagedListAsync(
				pageNumber, pageSize,
				nameof(Tag.Name), "DESC",
				cancellationToken);
		}


		public async Task<IPagedList<T>> GetPagedTagsAsync<T>(
			Func<IQueryable<Tag>,
			IQueryable<T>> mapper,
			IPagingParams pagingParams,
			string name = null,
			CancellationToken cancellationToken = default)
		{
			var tagQuery = _context.Set<Tag>().AsNoTracking();

			if (!string.IsNullOrEmpty(name))
			{
				tagQuery = tagQuery.Where(t => t.Name.Contains(name));
			}

			return await mapper(tagQuery)
				.ToPagedListAsync(pagingParams, cancellationToken);
		}

		public async Task<bool> AddOrUpdateAsync(
			Tag tag,
			CancellationToken cancellationToken = default)
		{
			if (tag.Id > 0)
			{
				_context.Tags.Update(tag);
				_memoryCache.Remove($"tag.by-id.{tag.Id}");
			}
			else
			{
				_context.Tags.Add(tag);
			}

			return await _context.SaveChangesAsync(cancellationToken) > 0;
		}

		public async Task<bool> DeleteTagAsync(
			int tagId,
			CancellationToken cancellationToken = default)
		{
			return await _context.Tags
				.Where(t => t.Id == tagId)
				.ExecuteDeleteAsync(cancellationToken) > 0;
		}

		public async Task<bool> IsTagSlugExistedAsync(
			int tagId,
			string slug,
			CancellationToken cancellationToken = default)
		{
			return await _context.Tags
				.AnyAsync(t => t.Id != tagId && t.UrlSlug == slug, cancellationToken);
		}

		private IQueryable<Tag> FilterTags(PostQuery condition)
		{
			IQueryable<Tag> tags = _context.Tags;

			if (!string.IsNullOrWhiteSpace(condition.Keyword))
			{
				tags = tags.Where(c => c.Name.Contains(condition.Keyword));
			}

			if (!string.IsNullOrWhiteSpace(condition.TagSlug))
			{
				tags = tags.Where(c => c.UrlSlug.Contains(condition.TagSlug));
			}


			return tags;
		}
	}
}
