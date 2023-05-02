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
	public interface IGameRepository
	{
		Task<Game> GetGameBySlugAsync(
			string slug,
			CancellationToken cancellationToken = default);

		Task<Game> GetCachedGameBySlugAsync(
			string slug,
			CancellationToken cancellationToken = default);

		Task<Game> GetGameByIdAsync(int gameId);

		Task<Game> GetCachedGameByIdAsync(int gameId);

		Task<IList<GameItem>> GetGamesAsync(
			CancellationToken cancellationToken = default);

		Task<IPagedList<GameItem>> GetPagedGamesAsync(
			IPagingParams pagingParams,
			string name = null,
			CancellationToken cancellationToken = default);

		Task<IPagedList<Game>> GetPagedGamesQueryAsync(
			PostQuery condition,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default);

		Task<IPagedList<T>> GetPagedGamesAsync<T>(
			Func<IQueryable<Game>, IQueryable<T>> mapper,
			IPagingParams pagingParams,
			string name = null,
			CancellationToken cancellationToken = default);

		Task<bool> AddOrUpdateAsync(
			Game game, 
			CancellationToken cancellationToken = default);

		Task<bool> DeleteGameAsync(
			int gameId,
			CancellationToken cancellationToken = default);

		Task<bool> IsGameSlugExistedAsync(
			int gameId,
			string slug,
			CancellationToken cancellationToken = default);

		Task<bool> SetImageUrlAsync(
			int gameId, string imageUrl,
			CancellationToken cancellationToken = default);

		Task<IList<Game>> GetPopularGamesAsync(
			int numGames,
			CancellationToken cancellationToken = default);
	}
}
