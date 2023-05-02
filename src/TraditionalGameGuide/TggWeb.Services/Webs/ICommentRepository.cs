using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TggWeb.Core.Contracts;
using TggWeb.Core.DTO;
using TggWeb.Core.Entities;

namespace TggWeb.Services.Webs
{
	public interface ICommentRepository
	{
		Task<IList<CommentItem>> GetCommentsAsync(
			CancellationToken cancellationToken = default);

		Task<IPagedList<CommentItem>> GetPagedCommentsAsync(
			IPagingParams pagingParams,
			string content = null,
			CancellationToken cancellationToken = default);

		Task<Comment> GetCommentByIdAsync(
			int commentId,
			CancellationToken cancellationToken = default);

		Task<Comment> GetCachedCommentByIdAsync(int commentId);

		Task<IPagedList<Comment>> GetPagedCommentsByPostIdAsync(
			int postId,
			IPagingParams pagingParams,
			CancellationToken cancellationToken = default);

		Task<Comment> CreateCommentAsync(
			Comment comment,
			CancellationToken cancellationToken = default);

		Task<bool> UpdateCommentAsync(
			Comment comment,
			CancellationToken cancellationToken = default);

		Task<bool> DeleteCommentAsync(
			int commentId,
			CancellationToken cancellationToken = default);

	}
}
