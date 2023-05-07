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
	public static class TagEndpoints
	{

		public static WebApplication MapTagEndpoints(
			this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/tags");

			routeGroupBuilder.MapGet("/", GetTags)
				.WithName("GetTags")
				.Produces<ApiResponse<PaginationResult<TagItem>>>();

			routeGroupBuilder.MapGet("/id/{id:int}", GetTagDetails)
				.WithName("GetTagById")
				.Produces< ApiResponse<TagItem>>();

			routeGroupBuilder.MapGet(
					"/posts/slug/{slug:regex(^[a-z0-9 -]+$)}",
					GetPostsByTagSlug)
				.WithName("GetPostsByTagSlug")
				.Produces<ApiResponse<PaginationResult<PostDto>>>();

			routeGroupBuilder.MapPost("/", AddTag)
				.AddEndpointFilter<ValidatorFilter<TagEditModel>>()
				.WithName("AddNewTag")
				.Produces(401)
				.Produces<ApiResponse<TagDto>>();

			routeGroupBuilder.MapPut("/{id:int}", UpdateTag)
				.WithName("UpdateAnTag")
				.AddEndpointFilter<ValidatorFilter<TagEditModel>>()
				.Produces(401)
				.Produces<ApiResponse<TagItem>>();

			routeGroupBuilder.MapDelete("/{id:int}", DeleteTag)
				.WithName("DeleteAnTag")
				.Produces(204)
				.Produces(404);

			

			return app;
		}

		private static async Task<IResult> GetTags(
			[AsParameters] TagFilterModel model,
			[FromServices] ITagRepository tagRepository)
		{
			var TagsList = await tagRepository
				.GetPagedTagsAsync(model, model.Name);

			var paginationResult =
				new PaginationResult<TagItem>(TagsList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> GetTagDetails(
			int id,
			[FromServices] ITagRepository tagRepository,
			[FromServices] IMapper mapper)
		{
			var Tag = await tagRepository.GetCachedTagByIdAsync(id);
			
			return Tag == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, 
				$"Could not find Tag with Id {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(Tag)));
		}


		private static async Task<IResult> GetPostsByTagSlug(
			[FromRoute] string slug,
			[AsParameters] PagingModel pagingModel,
			[FromServices] IWebRepository webRepository)
		{
			var postQuery = new PostQuery()
			{
				TagSlug = slug,
				PublishedOnly = true
			};

			var postsList = await webRepository.GetPagedPostsAsync(
				postQuery, pagingModel,
				posts => posts.ProjectToType<PostDto>());

			var paginationResult = new PaginationResult<PostDto>(postsList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> AddTag(
			[AsParameters] TagEditModel model,
			[FromServices] ITagRepository tagRepository,
			[FromServices] IMapper mapper)
		{
			if (await tagRepository
				.IsTagSlugExistedAsync(0, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
				$"Slug '{model.UrlSlug}' already exist"));
				
			}

			var Tag = mapper.Map<Tag>(model);
			await tagRepository.AddOrUpdateAsync(Tag);

			return Results.Ok(ApiResponse.Success(
				mapper.Map<TagDto>(Tag),
				HttpStatusCode.Created));
		}


		private static async Task<IResult> UpdateTag(
			[FromRoute] int id,
			[AsParameters] TagEditModel model,
			[FromServices] IValidator<TagEditModel> validator,
			[FromServices] ITagRepository tagRepository,
			[FromServices] IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.BadRequest, validationResult));
			}

			if (await tagRepository.IsTagSlugExistedAsync(
				id, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.Conflict,
					$"Slug '{model.UrlSlug}' already exist"));
			}
			
			var Tag = mapper.Map<Tag>(model);
			Tag.Id = id;

			return await tagRepository.AddOrUpdateAsync(Tag)
				? Results.Ok(ApiResponse.Success("Tag is updated",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find Tag"));
		}

		private static async Task<IResult> DeleteTag(
			[FromRoute] int id,
			[FromServices] ITagRepository tagRepository)
		{
			return await tagRepository.DeleteTagAsync(id)
				? Results.Ok(ApiResponse.Success("Tag is deleted",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find Tag"));
		}

	}
}
