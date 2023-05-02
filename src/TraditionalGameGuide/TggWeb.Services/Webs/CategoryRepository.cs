using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TggWeb.Core.Contracts;
using TggWeb.Core.DTO;
using TggWeb.Core.Entities;
using TggWeb.Data.Contexts;
using TggWeb.Services.Extensions;

namespace TggWeb.Services.Webs
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly WebDbContext _context;
		private readonly IMemoryCache _memoryCache;

		public CategoryRepository(WebDbContext context, IMemoryCache memoryCache)
		{
			_context = context;
			_memoryCache = memoryCache;
		}

		public async Task<Category> GetCategoryBySlugAsync(
			string slug,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Category>()
				.FirstOrDefaultAsync(c => c.UrlSlug == slug, cancellationToken);
		}

		public async Task<Category> GetCachedCategoryBySlugAsync(
			string slug,
			CancellationToken cancellationToken = default)
		{
			return await _memoryCache.GetOrCreateAsync(
				$"category.by-slug.{slug}",
				async (entry) =>
				{
					entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
					return await GetCategoryBySlugAsync(slug, cancellationToken);
				});
		}

		public async Task<Category> GetCategoryByIdAsync(int categoryId)
		{
			return await _context.Set<Category>().FindAsync(categoryId);
		}

		public async Task<Category> GetCachedCategoryByIdAsync(int categoryId)
		{
			return await _memoryCache.GetOrCreateAsync(
				$"category.by-id.{categoryId}",
				async (entry) =>
				{
					entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
					return await GetCategoryByIdAsync(categoryId);
				});
		}

		public async Task<IList<CategoryItem>> GetCategoriesAsync(
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Category>()
				.OrderBy(x => x.Name)
				.Select(c => new CategoryItem()
				{
					Id = c.Id,

					Name = c.Name,

					UrlSlug = c.UrlSlug,

					Description = c.Description,

					GameCount = c.Games.Count(),

					PostCount = c.Games.SelectMany(g => g.Posts).Count(p => p.Published)
				})
				.ToListAsync(cancellationToken);
		}

		public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
			IPagingParams pagingParams,
			string name = null,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Category>()
				.AsNoTracking()
				.WhereIf(!string.IsNullOrWhiteSpace(name),
					c => c.Name.Contains(name))
				.Select(c => new CategoryItem()
				{
					Id = c.Id,

					Name = c.Name,

					UrlSlug = c.UrlSlug,

					Description = c.Description,

					GameCount = c.Games.Count(),

					PostCount = c.Games.SelectMany(g => g.Posts).Count(p => p.Published)
				})
				.ToPagedListAsync(pagingParams, cancellationToken);
		}

		public async Task<IPagedList<Category>> GetPagedCategoriesQueryAsync(
			PostQuery condition,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default)
		{
			return await FilterCategories(condition).ToPagedListAsync(
				pageNumber, pageSize,
				nameof(Category.Name), "DESC",
				cancellationToken);
		}

		public async Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
			Func<IQueryable<Category>, 
			IQueryable<T>> mapper,
			IPagingParams pagingParams,
			string name = null,
			CancellationToken cancellationToken = default)
		{
			var categoryQuery = _context.Set<Category>().AsNoTracking();

			if (!string.IsNullOrEmpty(name))
			{
				categoryQuery = categoryQuery.Where(c => c.Name.Contains(name));
			}

			return await mapper(categoryQuery)
				.ToPagedListAsync(pagingParams, cancellationToken);
		}

		public async Task<bool> AddOrUpdateAsync(
			Category category,
			CancellationToken cancellationToken = default)
		{
			if (category.Id > 0)
			{
				_context.Categories.Update(category);
				_memoryCache.Remove($"Category.by-id.{category.Id}");
			}
			else
			{
				_context.Categories.Add(category);
			}

			return await _context.SaveChangesAsync(cancellationToken) > 0;
		}

		public async Task<bool> DeleteCategoryAsync(
			int categoryId,
			CancellationToken cancellationToken = default)
		{
			return await _context.Categories
				.Where(c => c.Id == categoryId)
				.ExecuteDeleteAsync(cancellationToken) > 0;
		}

		public async Task<bool> IsCategorySlugExistedAsync(
			int categoryId,
			string slug,
			CancellationToken cancellationToken = default)
		{
			return await _context.Categories
				.AnyAsync(c => c.Id != categoryId && c.UrlSlug == slug, cancellationToken);
		}


		public async Task<IList<CategoryItem>> GetPopularCategoriesAsync(
			int numCategories,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Category>()
				.Select(c => new CategoryItem
				{
					Id = c.Id,
					Name = c.Name,
					UrlSlug = c.UrlSlug,
					Description = c.Description,
					GameCount = c.Games.Count(),
					PostCount = c.Games.SelectMany(g => g.Posts).Count(p => p.Published)
				})
				.OrderByDescending(c => c.PostCount)
				.Take(numCategories)
				.ToListAsync(cancellationToken);
		}




		private IQueryable<Category> FilterCategories(PostQuery condition)
		{
			IQueryable<Category> categories = _context.Categories;

			if (!string.IsNullOrWhiteSpace(condition.Keyword))
			{
				categories = categories.Where(c => c.Name.Contains(condition.Keyword));
			}

			if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
			{
				categories = categories.Where(c => c.UrlSlug.Contains(condition.CategorySlug));
			}


			return categories;
		}

		public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default)

		{
			var categoryQuery = _context.Set<Category>().Select(c => new CategoryItem()

			{

				Id = c.Id,
				Name = c.Name,
				UrlSlug = c.UrlSlug,
				Description = c.Description,
				GameCount = c.Games.Count(),
				PostCount = c.Games.SelectMany(g => g.Posts).Count(p => p.Published)
			});

			return await categoryQuery.ToPagedListAsync(pageNumber, pageSize);
		}
	}
}
