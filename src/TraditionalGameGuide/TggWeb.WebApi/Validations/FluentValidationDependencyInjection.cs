using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace TggWeb.WebApi.Validations
{
	public static class FluentValidationDependencyInjection
	{
		public static WebApplicationBuilder ConfigureFluentValidation(
			this WebApplicationBuilder builder)
		{
			
			builder.Services.AddValidatorsFromAssembly(
				Assembly.GetExecutingAssembly());

			return builder;
		}
	}
}
