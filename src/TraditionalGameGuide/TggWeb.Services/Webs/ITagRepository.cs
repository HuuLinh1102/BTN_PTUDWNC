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
	public interface ITagRepository
	{
		Task<Tag> GetTagBySlugAsync(
			string slug,
			CancellationToken cancellationToken = default);

		Task<Tag> GetCachedTagBySlugAsync(
			string slug,
			CancellationToken cancellationToken = default);

		Task<Tag> GetTagByIdAsync(int tagId);

		Task<Tag> GetCachedTagByIdAsync(int tagId);

		Task<IList<TagItem>> GetTagsAsync(
			CancellationToken cancellationToken = default);

		Task<IPagedList<TagItem>> GetPagedTagsAsync(
			IPagingParams pagingParams,
			string name = null,
			CancellationToken cancellationToken = default);

		Task<IPagedList<Tag>> GetPagedTagsQueryAsync(
			PostQuery condition,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default);

		Task<IPagedList<T>> GetPagedTagsAsync<T>(
			Func<IQueryable<Tag>,
			IQueryable<T>> mapper,
			IPagingParams pagingParams,
			string name = null,
			CancellationToken cancellationToken = default);

		Task<bool> AddOrUpdateAsync(
			Tag tag,
			CancellationToken cancellationToken = default);

		Task<bool> DeleteTagAsync(
			int tagId,
			CancellationToken cancellationToken = default);

		Task<bool> IsTagSlugExistedAsync(
			int tagId,
			string slug,
			CancellationToken cancellationToken = default);


	}
}
