using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.DTO;
using TggWeb.Core.Contracts;
using TggWeb.Core.Entities;

namespace TggWeb.Services.Web
{
	public interface IWebRepository
	{
		Task<Post> GetPostByIdAsync(
			int postId, bool includeDetails = false,
			CancellationToken cancellationToken = default);

		Task<Post> GetPostAsync(
			string slug,
			CancellationToken cancellationToken = default);

		Task<Post> GetPostDetailAsync(
			PostQuery condition,
			CancellationToken cancellationToken = default);

		Task<IPagedList<Post>> GetPagedPostsAsync(
			PostQuery condition,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default);

		Task<IList<Post>> GetPopularArticlesAsync(
			int numPosts, 
			CancellationToken cancellationToken = default);

		Task<IList<Post>> GetRandomArticlesAsync(
			int numPosts, 
			CancellationToken cancellationToken = default);

		Task<IList<Post>> GetRelatedPostsAsync(
			int postId, 
			int categoryId);

		Task<IList<Post>> GetPostsByMonthAsync(
			int year, 
			int month);

		Task IncreaseViewCountAsync(
			int postId, 
			CancellationToken cancellationToken = default);

		Task<bool> DeletePostAsync(
			int postId, 
			CancellationToken cancellationToken = default);

		Task<Post> CreateOrUpdatePostAsync(
			Post post, 
			IEnumerable<string> tags,
			CancellationToken cancellationToken = default);

		Task<bool> AddOrUpdateAsync(
			Post post,
			IEnumerable<string> tags,
			CancellationToken cancellationToken = default);

		Task<bool> IsPostSlugExistedAsync(
			int postId, string slug,
			CancellationToken cancellationToken = default);

		Task<bool> SetImageUrlAsync(
			int postId, string imageUrl,
			CancellationToken cancellationToken = default);

		Task<bool> TogglePublishedFlagAsync(
			int postId, 
			CancellationToken cancellationToken = default);
	}
}
