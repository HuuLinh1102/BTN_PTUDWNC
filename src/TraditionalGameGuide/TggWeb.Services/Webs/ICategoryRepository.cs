using TggWeb.Core.Contracts;
using TggWeb.Core.DTO;
using TggWeb.Core.Entities;

namespace TggWeb.Services.Webs
{
	public interface ICategoryRepository
	{
		Task<Category> GetCategoryBySlugAsync(
			string slug, 
			CancellationToken cancellationToken = default);

		Task<Category> GetCachedCategoryBySlugAsync(
			string slug, 
			CancellationToken cancellationToken = default);

		Task<Category> GetCategoryByIdAsync(int categoryId);

		Task<Category> GetCachedCategoryByIdAsync(int categoryId);

		Task<IList<CategoryItem>> GetCategoriesAsync(
			CancellationToken cancellationToken = default);

		Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
			IPagingParams pagingParams,
			string name = null,
			CancellationToken cancellationToken = default);

		Task<IPagedList<Category>> GetPagedCategoriesQueryAsync(
			PostQuery condition,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default);

		Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
			Func<IQueryable<Category>, IQueryable<T>> mapper,
			IPagingParams pagingParams,
			string name = null,
			CancellationToken cancellationToken = default);

		Task<bool> AddOrUpdateAsync(
			Category category, 
			CancellationToken cancellationToken = default);

		Task<bool> DeleteCategoryAsync(
			int categoryId,
			CancellationToken cancellationToken = default);

		Task<bool> IsCategorySlugExistedAsync(
			int categoryId,
			string slug,
			CancellationToken cancellationToken = default);

		Task<IList<CategoryItem>> GetPopularCategoriesAsync(
				int numCategories,
				CancellationToken cancellationToken = default);

		Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default);
	}
}
