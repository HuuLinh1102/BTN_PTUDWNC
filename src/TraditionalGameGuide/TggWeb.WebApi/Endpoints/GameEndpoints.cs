using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TggWeb.Core.Collections;
using TggWeb.Core.DTO;
using TggWeb.Core.Entities;
using TggWeb.Services.Webs;
using TggWeb.Services.Media;
using TggWeb.WebApi.Extensions;
using TggWeb.WebApi.Filters;
using TggWeb.WebApi.Models;

namespace TggWeb.WebApi.Endpoints
{
	public static class GameEndpoints
	{

		public static WebApplication MapGameEndpoints(
			this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/games");

			routeGroupBuilder.MapGet("/", GetGames)
				.WithName("GetGames")
				.Produces<ApiResponse<PaginationResult<GameItem>>>();

			routeGroupBuilder.MapGet("/id/{id:int}", GetGameDetails)
				.WithName("GetGameById")
				.Produces< ApiResponse<GameItem>>();

			routeGroupBuilder.MapGet("/posts/id/{id:int}", GetPostsByGame)
				.WithName("GetPostsByGame")
				.Produces<ApiResponse<IEnumerable<PostDto>>>()
				.Produces(404);

			routeGroupBuilder.MapGet(
					"/posts/slug/{slug:regex(^[a-z0-9 -]+$)}",
					GetPostsByGameSlug)
				.WithName("GetPostsByGameSlug")
				.Produces<ApiResponse<PaginationResult<PostDto>>>();

			routeGroupBuilder.MapPost("/", AddGame)
				.AddEndpointFilter<ValidatorFilter<GameEditModel>>()
				.WithName("AddNewGame")
				.Produces(401)
				.Produces<ApiResponse<GameDto>>();

			routeGroupBuilder.MapPost("/{id:int}/avatar", SetGamePicture)
				.WithName("SetGamePicture")
				.Accepts<IFormFile>("multipart/from-data")
				.Produces(401)
				.Produces<ApiResponse<GameItem>>();

			routeGroupBuilder.MapPut("/{id:int}", UpdateGame)
				.WithName("UpdateAnGame")
				.AddEndpointFilter<ValidatorFilter<GameEditModel>>()
				.Produces(401)
				.Produces<ApiResponse<GameItem>>();

			routeGroupBuilder.MapDelete("/{id:int}", DeleteGame)
				.WithName("DeleteAnGame")
				.Produces(204)
				.Produces(404);

			

			return app;
		}

		private static async Task<IResult> GetGames(
			[AsParameters] GameFilterModel model,
			[FromServices] IGameRepository gameRepository)
		{
			var gamesList = await gameRepository
				.GetPagedGamesAsync(model, model.Name);

			var paginationResult =
				new PaginationResult<GameItem>(gamesList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> GetGameDetails(
			int id,
			[FromServices] IGameRepository gameRepository,
			[FromServices] IMapper mapper)
		{
			var game = await gameRepository.GetCachedGameByIdAsync(id);
			
			return game == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, 
				$"Could not find game with Id {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<GameItem>(game)));
		}

		private static async Task<IResult> GetPostsByGame(
			int id,
			[AsParameters] PagingModel pagingModel,
			[FromServices] IWebRepository webRepository)
		{
			var postQuery = new PostQuery()
			{
				GameId = id,
				PublishedOnly = true
			};

			var postsList = await webRepository.GetPagedPostsAsync(
				postQuery, pagingModel,
				posts => posts.ProjectToType<PostDto>());

			var paginationResult = new PaginationResult<PostDto>(postsList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}


		private static async Task<IResult> GetPostsByGameSlug(
			[FromRoute] string slug,
			[AsParameters] PagingModel pagingModel,
			[FromServices] IWebRepository webRepository)
		{
			var postQuery = new PostQuery()
			{
				GameSlug = slug,
				PublishedOnly = true
			};

			var postsList = await webRepository.GetPagedPostsAsync(
				postQuery, pagingModel,
				posts => posts.ProjectToType<PostDto>());

			var paginationResult = new PaginationResult<PostDto>(postsList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> AddGame(
			[AsParameters] GameEditModel model,
			[FromServices] IGameRepository gameRepository,
			[FromServices] IMapper mapper)
		{
			if (await gameRepository
				.IsGameSlugExistedAsync(0, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
				$"Slug '{model.UrlSlug}' already exist"));
				
			}

			var game = mapper.Map<Game>(model);
			await gameRepository.AddOrUpdateAsync(game);

			return Results.Ok(ApiResponse.Success(
				mapper.Map<GameDto>(game),
				HttpStatusCode.Created));
		}


		private static async Task<IResult> SetGamePicture(
			[FromRoute] int id, IFormFile imageFile,
			[FromServices] IGameRepository gameRepository,
			[FromServices] IMediaManager mediaManager)
		{
			var imageUrl = await mediaManager.SaveFileAsync(
				imageFile.OpenReadStream(),
				imageFile.FileName, imageFile.ContentType);

			if (string.IsNullOrWhiteSpace(imageUrl))
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.BadRequest, "File could not be saved"));
			}

			await gameRepository.SetImageUrlAsync(id, imageUrl);
			return Results.Ok(ApiResponse.Success(imageUrl));
		}


		private static async Task<IResult> UpdateGame(
			[FromRoute] int id,
			[AsParameters] GameEditModel model,
			[FromServices] IValidator<GameEditModel> validator,
			[FromServices] IGameRepository gameRepository,
			[FromServices] IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.BadRequest, validationResult));
			}

			if (await gameRepository.IsGameSlugExistedAsync(
				id, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.Conflict,
					$"Slug '{model.UrlSlug}' already exist"));
			}
			
			var game = mapper.Map<Game>(model);
			game.Id = id;

			return await gameRepository.AddOrUpdateAsync(game)
				? Results.Ok(ApiResponse.Success("Game is updated",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find game"));
		}

		private static async Task<IResult> DeleteGame(
			[FromRoute] int id,
			[FromServices] IGameRepository gameRepository)
		{
			return await gameRepository.DeleteGameAsync(id)
				? Results.Ok(ApiResponse.Success("Game is deleted",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find game"));
		}

		private static async Task<IResult> GetTopGames(
			[FromRoute] int limit,
			[FromServices] IGameRepository gameRepository,
			[FromServices] IMapper mapper)
		{
			var games = await gameRepository
				.GetPopularGamesAsync(limit);

			var gamesList = mapper.Map<IEnumerable<GameItem>>(games);
			return Results.Ok(ApiResponse.Success(gamesList));
		}

	}
}
