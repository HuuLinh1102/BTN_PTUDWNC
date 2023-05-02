
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TggWeb.Core.Contracts;
using TggWeb.Core.DTO;
using TggWeb.Core.Entities;
using TggWeb.Data.Contexts;
using TggWeb.Services.Extensions;

namespace TggWeb.Services.Webs;

public class GameRepository : IGameRepository
{
	private readonly WebDbContext _context;
	private readonly IMemoryCache _memoryCache;

	public GameRepository(WebDbContext context, IMemoryCache memoryCache)
	{
		_context = context;
		_memoryCache = memoryCache;
	}

	public async Task<Game> GetGameBySlugAsync(
		string slug, 
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Game>()
			.FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
	}

	public async Task<Game> GetCachedGameBySlugAsync(
		string slug, 
		CancellationToken cancellationToken = default)
	{
		return await _memoryCache.GetOrCreateAsync(
			$"author.by-slug.{slug}",
			async (entry) =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
				return await GetGameBySlugAsync(slug, cancellationToken);
			});
	}

	public async Task<Game> GetGameByIdAsync(int gameId)
	{
		return await _context.Set<Game>().FindAsync(gameId);
	}

	public async Task<Game> GetCachedGameByIdAsync(int gameId)
	{
		return await _memoryCache.GetOrCreateAsync(
			$"author.by-id.{gameId}",
			async (entry) =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
				return await GetGameByIdAsync(gameId);
			});
	}

	public async Task<IList<GameItem>> GetGamesAsync(
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Game>()
			.OrderBy(a => a.Name)
			.Select(a => new GameItem()
			{
				Id = a.Id,
				Name = a.Name,
				ImageUrl = a.ImageUrl,
				UrlSlug = a.UrlSlug,
				Category = a.Category,
				PostCount = a.Posts.Count(p => p.Published)
			})
			.ToListAsync(cancellationToken);
	}

	public async Task<IPagedList<GameItem>> GetPagedGamesAsync(
		IPagingParams pagingParams,
		string name = null,
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Game>()
			.AsNoTracking()
			.WhereIf(!string.IsNullOrWhiteSpace(name), 
				x => x.Name.Contains(name))
			.Select(a => new GameItem()
			{
				Id = a.Id,
				Name = a.Name,
				ImageUrl = a.ImageUrl,
				UrlSlug = a.UrlSlug,
				Category = a.Category,
				PostCount = a.Posts.Count(p => p.Published)
			})
			.ToPagedListAsync(pagingParams, cancellationToken);
	}

	public async Task<IPagedList<Game>> GetPagedGamesQueryAsync(
		PostQuery condition,
		int pageNumber = 1,
		int pageSize = 10,
		CancellationToken cancellationToken = default)
	{
		return await FilterGames(condition).ToPagedListAsync(
			pageNumber, pageSize,
			nameof(Game.Name), "DESC",
			cancellationToken);
	}

	public async Task<IPagedList<T>> GetPagedGamesAsync<T>(
		Func<IQueryable<Game>, 
		IQueryable<T>> mapper,
		IPagingParams pagingParams,
		string name = null,
		CancellationToken cancellationToken = default)
	{
		var gameQuery = _context.Set<Game>().AsNoTracking();

		if (!string.IsNullOrEmpty(name))
		{
			gameQuery = gameQuery.Where(x => x.Name.Contains(name));
		}

		return await mapper(gameQuery)
			.ToPagedListAsync(pagingParams, cancellationToken);
	}

	public async Task<bool> AddOrUpdateAsync(
		Game game, 
		CancellationToken cancellationToken = default)
	{
		if (game.Id > 0)
		{
			_context.Games.Update(game);
			_memoryCache.Remove($"game.by-id.{game.Id}");
		}
		else
		{
			_context.Games.Add(game);
		}

		return await _context.SaveChangesAsync(cancellationToken) > 0;
	}
	
	public async Task<bool> DeleteGameAsync(
		int	gameId, 
		CancellationToken cancellationToken = default)
	{
		return await _context.Games
			.Where(x => x.Id == gameId)
			.ExecuteDeleteAsync(cancellationToken) > 0;
	}

	public async Task<bool> IsGameSlugExistedAsync(
		int gameId, 
		string slug, 
		CancellationToken cancellationToken = default)
	{
		return await _context.Games
			.AnyAsync(x => x.Id != gameId && x.UrlSlug == slug, cancellationToken);
	}

	public async Task<bool> SetImageUrlAsync(
		int gameId, string imageUrl,
		CancellationToken cancellationToken = default)
	{
		return await _context.Games
			.Where(x => x.Id == gameId)
			.ExecuteUpdateAsync(x => 
				x.SetProperty(a => a.ImageUrl, a => imageUrl), 
				cancellationToken) > 0;
	}

	public async Task<IList<Game>> GetPopularGamesAsync(
			int numGames,
			CancellationToken cancellationToken = default)
	{
		return await _context.Set<Game>()
			.Include(x => x.Posts)
			.OrderByDescending(a => a.Posts.Count())
			.Take(numGames)
			.ToListAsync(cancellationToken);
	}


	private IQueryable<Game> FilterGames(PostQuery condition)
	{
		IQueryable<Game> games = _context.Games;

		if (!string.IsNullOrWhiteSpace(condition.Keyword))
		{
			games = games.Where(c => c.Name.Contains(condition.Keyword) ||
									 c.Description.Contains(condition.Keyword));
		}

		if (!string.IsNullOrWhiteSpace(condition.GameSlug))
		{
			games = games.Where(c => c.UrlSlug.Contains(condition.GameSlug));
		}


		return games;
	}
}