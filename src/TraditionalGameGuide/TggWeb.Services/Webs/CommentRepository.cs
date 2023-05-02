using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TggWeb.Core.Contracts;
using TggWeb.Core.DTO;
using TggWeb.Core.Entities;
using TggWeb.Data.Contexts;
using TggWeb.Services.Extensions;

namespace TggWeb.Services.Webs
{
	public class CommentRepository : ICommentRepository
	{
		private readonly WebDbContext _context;
		private readonly IMemoryCache _memoryCache;

		public CommentRepository(WebDbContext context, IMemoryCache memoryCache)
		{
			_context = context;
			_memoryCache = memoryCache;
		}

		public async Task<IList<CommentItem>> GetCommentsAsync(
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Comment>()
				.OrderBy(s => s.Content)
				.Select(s => new CommentItem()
				{
					Id = s.Id,
					Content = s.Content,
					SubscriberId = s.SubscriberId,
					CreatedDate = s.CreatedDate,
					IsApproved = s.IsApproved,
					PostId = s.PostId
					
				})
				.ToListAsync(cancellationToken);
		}

		public async Task<IPagedList<CommentItem>> GetPagedCommentsAsync(
			IPagingParams pagingParams,
			string content = null,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Comment>()
				.AsNoTracking()
				.WhereIf(!string.IsNullOrWhiteSpace(content),
					s => s.Content.Contains(content))
				.Select(s => new CommentItem()
				{
					Id = s.Id,
					Content = s.Content,
					SubscriberId = s.SubscriberId,
					CreatedDate = s.CreatedDate,
					IsApproved = s.IsApproved,
					PostId = s.PostId
				})
				.ToPagedListAsync(pagingParams, cancellationToken);
		}


		public async Task<Comment> GetCommentByIdAsync(
			int commentId, 
			CancellationToken cancellationToken = default)
		{
			var comment = await _context.Comments
				.AsNoTracking()
				.Include(c => c.Subscriber)
				.FirstOrDefaultAsync(c => c.Id == commentId, cancellationToken);
			return comment;
		}

		public async Task<Comment> GetCachedCommentByIdAsync(int commentId)
		{
			return await _memoryCache.GetOrCreateAsync(
				$"comment.by-id.{commentId}",
				async (entry) =>
				{
					entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
					return await GetCommentByIdAsync(commentId);
				});
		}

		public async Task<IPagedList<Comment>> GetPagedCommentsByPostIdAsync(
			int postId,
			IPagingParams pagingParams,
			CancellationToken cancellationToken = default)
		{
			return await _context.Comments
				.AsNoTracking()
				.Include(c => c.Post)
				.Include(c => c.Subscriber)
				.Where(c => c.PostId == postId)
				.OrderByDescending(c => c.CreatedDate)
				.ToPagedListAsync(pagingParams, cancellationToken);
		}



		public async Task<Comment> CreateCommentAsync(
			Comment comment, 
			CancellationToken cancellationToken = default)
		{
			await _context.Comments.AddAsync(comment, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);
			return comment;
		}

		public async Task<bool> UpdateCommentAsync(
			Comment comment, 
			CancellationToken cancellationToken = default)
		{
			_context.Comments.Update(comment);
			return await _context.SaveChangesAsync(cancellationToken) > 0;
		}


		public async Task<bool> DeleteCommentAsync(
		int commentId,
		CancellationToken cancellationToken = default)
		{
			return await _context.Comments
				.Where(x => x.Id == commentId)
				.ExecuteDeleteAsync(cancellationToken) > 0;
		}

	}
}
