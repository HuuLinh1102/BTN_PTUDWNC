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
	public static class SubscriberEndpoints
	{

		public static WebApplication MapSubscriberEndpoints(
			this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/Subscribers");

			routeGroupBuilder.MapGet("/", GetSubscribers)
				.WithName("GetSubscribers")
				.Produces<ApiResponse<PaginationResult<SubscriberItem>>>();

			routeGroupBuilder.MapGet("/id/{id:int}", GetSubscriberDetails)
				.WithName("GetSubscriberById")
				.Produces< ApiResponse<SubscriberItem>>();

			routeGroupBuilder.MapPost("/add", AddSubscriber)
				.AddEndpointFilter<ValidatorFilter<SubscriberEditModel>>()
				.WithName("AddNewSubscriber")
				.Produces(401)
				.Produces<ApiResponse<SubscriberItem>>();

			routeGroupBuilder.MapPut("/update/{id:int}", UpdateSubscriber)
				.WithName("UpdateAnSubscriber")
				.AddEndpointFilter<ValidatorFilter<SubscriberEditModel>>()
				.Produces(401)
				.Produces<ApiResponse<SubscriberItem>>();

			routeGroupBuilder.MapDelete("/delete/{id:int}", DeleteSubscriber)
				.WithName("DeleteAnSubscriber")
				.Produces(204)
				.Produces(404);

			

			return app;
		}

		private static async Task<IResult> GetSubscribers(
			[AsParameters] SubscriberFilterModel model,
			[FromServices] ISubscriberRepository subscriberRepository)
		{
			var SubscribersList = await subscriberRepository
				.GetPagedSubscribersAsync(model, model.Email);

			var paginationResult =
				new PaginationResult<SubscriberItem>(SubscribersList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> GetSubscriberDetails(
			int id,
			[FromServices] ISubscriberRepository subscriberRepository,
			[FromServices] IMapper mapper)
		{
			var Subscriber = await subscriberRepository.GetCachedSubscriberByIdAsync(id);
			
			return Subscriber == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, 
				$"Could not find Subscriber with Id {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<SubscriberItem>(Subscriber)));
		}

		private static async Task<IResult> AddSubscriber(
			[FromServices] SubscriberEditModel model,
			[FromServices] ISubscriberRepository subscriberRepository,
			[FromServices] IMapper mapper)
		{
			if (await subscriberRepository
				.IsSubscriberEmailExistedAsync(0, model.Email))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
				$"Email '{model.Email}' already registered"));
				
			}

			var Subscriber = mapper.Map<Subscriber>(model);
			await subscriberRepository.AddOrUpdateAsync(Subscriber);

			return Results.Ok(ApiResponse.Success(
				mapper.Map<SubscriberItem>(Subscriber),
				HttpStatusCode.Created));
		}


		private static async Task<IResult> UpdateSubscriber(
			[FromRoute] int id,
			[FromServices] SubscriberEditModel model,
			[FromServices] IValidator<SubscriberEditModel> validator,
			[FromServices] ISubscriberRepository subscriberRepository,
			[FromServices] IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.BadRequest, validationResult));
			}

			if (await subscriberRepository.IsSubscriberEmailExistedAsync(
				id, model.Email))
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.Conflict,
					$"Email '{model.Email}' already registered"));
			}
			
			var Subscriber = mapper.Map<Subscriber>(model);
			Subscriber.Id = id;

			return await subscriberRepository.AddOrUpdateAsync(Subscriber)
				? Results.Ok(ApiResponse.Success("Subscriber is updated",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find Subscriber"));
		}

		private static async Task<IResult> DeleteSubscriber(
			[FromRoute] int id,
			[FromServices] ISubscriberRepository subscriberRepository)
		{
			return await subscriberRepository.DeleteSubscriberAsync(id)
				? Results.Ok(ApiResponse.Success("Subscriber is deleted",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find Subscriber"));
		}

	}
}
