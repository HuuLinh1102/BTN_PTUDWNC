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
using TggWeb.Core.Contracts;

namespace TggWeb.WebApi.Endpoints
{
	public static class CommentEndpoints
	{

		public static WebApplication MapCommentEndpoints(
			this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/comments");

			routeGroupBuilder.MapGet("/", GetComments)
				.WithName("GetComments")
				.Produces<ApiResponse<CommentItem>>();

			routeGroupBuilder.MapGet("/{id:int}", GetCommentDetails)
				.WithName("GetCommentDetails")
				.Produces< ApiResponse<CommentDto>>();

			routeGroupBuilder.MapPost("/", AddComment)
				.AddEndpointFilter<ValidatorFilter<CommentEditModel>>()
				.WithName("AddNewComment")
				.Produces(401)
				.Produces<ApiResponse<CommentItem>>();

			routeGroupBuilder.MapPut("/{id:int}", UpdateComment)
				.WithName("UpdateAnComment")
				.AddEndpointFilter<ValidatorFilter<CommentEditModel>>()
				.Produces(401)
				.Produces<ApiResponse<CommentItem>>();

			routeGroupBuilder.MapDelete("/{id:int}", DeleteComment)
				.WithName("DeleteAnComment")
				.Produces(204)
				.Produces(404);

			

			return app;
		}

		private static async Task<IResult> GetComments(
			[FromServices] ICommentRepository commentRepository)
		{
			var commentsList = await commentRepository
				.GetCommentsAsync();

			return Results.Ok(ApiResponse.Success(commentsList));
		}


		private static async Task<IResult> GetCommentDetails(
			[FromRoute] int id,
			[FromServices] ICommentRepository commentRepository,
			[FromServices] IMapper mapper)
		{
			var comment = await commentRepository.GetCachedCommentByIdAsync(id);
			
			return comment == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, 
				$"Could not find Comment with Id {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<CommentDto>(comment)));
		}

		private static async Task<IResult> AddComment(
			[FromServices] CommentEditModel model,
			[FromServices] ICommentRepository commentRepository,
			[FromServices] IMapper mapper)
		{

			var comment = mapper.Map<Comment>(model);
			await commentRepository.CreateCommentAsync(comment);

			return Results.Ok(ApiResponse.Success(
				mapper.Map<CommentItem>(comment),
				HttpStatusCode.Created));
		}


		private static async Task<IResult> UpdateComment(
			[FromRoute] int id,
			[FromServices] CommentEditModel model,
			[FromServices] IValidator<CommentEditModel> validator,
			[FromServices] ICommentRepository commentRepository,
			[FromServices] IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.BadRequest, validationResult));
			}

			var comment = mapper.Map<Comment>(model);
			comment.Id = id;

			return await commentRepository.UpdateCommentAsync(comment)
				? Results.Ok(ApiResponse.Success("Comment is updated",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find comment"));
		}

		private static async Task<IResult> DeleteComment(
			[FromRoute] int id,
			[FromServices] ICommentRepository commentRepository)
		{
			return await commentRepository.DeleteCommentAsync(id)
				? Results.Ok(ApiResponse.Success("Comment is deleted",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find comment"));
		}

	}
}
