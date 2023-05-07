using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TggWeb.Core.DTO;
using TggWeb.Core.Collections;
using TggWeb.Core.Entities;
using TggWeb.Services.Media;
using TggWeb.Services.Webs;
using TggWeb.WebApi.Filters;
using TggWeb.WebApi.Models;

namespace TggWeb.WebApi.Endpoints
{
	public static class PostEndpoints
	{
		public static WebApplication MapPostEndpoints(
			this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/posts");

			routeGroupBuilder.MapGet("/", GetPosts)
				.WithName("GetPosts")
				.Produces<ApiResponse<PaginationResult<PostItem>>>();

			routeGroupBuilder.MapGet("/featured/{limit:int}", GetTopPosts)
				.WithName("GetTopPosts")
				.Produces<ApiResponse<IEnumerable<PostDto>>>()
				.Produces(404);

			routeGroupBuilder.MapGet("/random/{limit:int}", GetRandomPosts)
				.WithName("GetRandomPosts")
				.Produces<ApiResponse<IEnumerable<PostDto>>>()
				.Produces(404);


			routeGroupBuilder.MapGet("/{id:int}", GetPostDetailsById)
				.WithName("GetPostDetailsById")
				.Produces<ApiResponse<PostDetail>>()
				.Produces(404);

			routeGroupBuilder.MapGet("/byslug/{slug:regex(^[a-z0-9 -]+$)}", GetPostDetailsBySlug)
				.WithName("GetPostDetailsBySlug")
				.Produces<ApiResponse<PostItem>>()
				.Produces(404);

			routeGroupBuilder.MapPost("/", AddPost)
				.AddEndpointFilter<ValidatorFilter<PostEditModel>>()
				.WithName("AddNewPost")
				.Produces(401)
				.Produces<ApiResponse<PostDetail>>();

			routeGroupBuilder.MapPost("/{id:int}/picture", SetPostPicture)
				.WithName("SetPostPicture")
				.Accepts<IFormFile>("multipart/from-data")
				.Produces(401)
				.Produces<ApiResponse<PostItem>>();

			routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
				.WithName("UpdateAnPost")
				.AddEndpointFilter<ValidatorFilter<PostEditModel>>()
				.Produces(401)
				.Produces<ApiResponse<PostDto>>();

			routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
				.WithName("DeleteAnPost")
				.Produces(204)
				.Produces(404);

			return app;
		}

		private static async Task<IResult> GetPosts(
			[AsParameters] PostFilterModel model,
			IMapper mapper,
			[FromServices] IWebRepository webRepository,
			[AsParameters] PagingModel pagingModel)
		{
			var postQuery = mapper.Map<PostQuery>(model);

			var postsList = await webRepository.GetPagedPostsAsync(
				postQuery, pagingModel,
				posts => posts.ProjectToType<PostDto>());

			var paginationResult = new PaginationResult<PostDto>(postsList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}
		private static async Task<IResult> GetTopPosts(
			int limit,
			[FromServices] IWebRepository webRepository,
			[FromServices] IMapper mapper)
		{
			var posts = await webRepository.GetPopularArticlesAsync(limit);
			if (posts == null)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find post"));
			}

			var postsList = mapper.Map<IEnumerable<PostDto>>(posts);
			return Results.Ok(ApiResponse.Success(postsList));
		}

		private static async Task<IResult> GetRandomPosts(
			int limit,
			[FromServices] IWebRepository webRepository,
			[FromServices] IMapper mapper)
		{
			var posts = await webRepository.GetRandomArticlesAsync(limit);
			if (posts == null)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find post"));
			}

			var postsList = mapper.Map<IEnumerable<PostDto>>(posts);
			return Results.Ok(ApiResponse.Success(postsList));
		}

		private static async Task<IResult> GetPostDetailsById(
			int id,
			[FromServices] IWebRepository webRepository,
			[FromServices] IMapper mapper)
		{
			var post = await webRepository.GetPostByIdAsync(id, true);
			return post == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				$"Couldn not find post with Id {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(post)));
		}

		private static async Task<IResult> GetPostDetailsBySlug(
			string slug,
			[FromServices] IWebRepository webRepository,
			[FromServices] IMapper mapper)
		{
			var post = await webRepository.GetPostAsync(slug);
			return post == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				$"Couldn not find post with slug {slug}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<PostItem>(post)));
		}

		private static async Task<IResult> AddPost(
			[AsParameters] PostEditModel model,
			[FromServices] IWebRepository webRepository,
			[FromServices] IMapper mapper)
		{
			if (await webRepository
				.IsPostSlugExistedAsync(0, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
				$"Slug '{model.UrlSlug}' already exist"));

			}

			var post = mapper.Map<Post>(model);
			post.PostedDate = DateTime.Now;
			await webRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTag());

			return Results.Ok(ApiResponse.Success(
				mapper.Map<PostDetail>(post),
				HttpStatusCode.Created));
		}

		private static async Task<IResult> SetPostPicture(
			[FromRoute] int id, IFormFile imageFile,
			[FromServices] IWebRepository webRepository,
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

			await webRepository.SetImageUrlAsync(id, imageUrl);
			return Results.Ok(ApiResponse.Success(imageUrl));
		}

		private static async Task<IResult> UpdatePost(
			[FromRoute] int id,
			[AsParameters] PostEditModel model,
			[FromServices] IValidator<PostEditModel> validator,
			[FromServices] IWebRepository webRepository,
			[FromServices] IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.BadRequest, validationResult));
			}

			if (await webRepository.IsPostSlugExistedAsync(
				id, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.Conflict,
					$"Slug '{model.UrlSlug}' already exist"));
			}

			var post = mapper.Map<Post>(model);

			return await webRepository.AddOrUpdateAsync(post, model.GetSelectedTag())
				? Results.Ok(ApiResponse.Success("Post is updated",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find post"));
		}

		private static async Task<IResult> DeletePost(
			[FromRoute] int id,
			[FromServices] IWebRepository webRepository)
		{
			return await webRepository.DeletePostAsync(id)
				? Results.Ok(ApiResponse.Success("Post is deleted",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find post"));
		}
	}
}
