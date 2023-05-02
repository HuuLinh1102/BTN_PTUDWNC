using TggWeb.WebApi.Endpoints;
using TggWeb.WebApi.Extensions;
using TggWeb.WebApi.Mapsters;
using TggWeb.WebApi.Validations;

var builder = WebApplication.CreateBuilder(args);
{
	builder
		.ConfigureCors()
		.ConfigureNLog()
		.ConfigureServices()
		.ConfigureSwaggerOpenApi()
		.ConfigureMapster()
		.ConfigureFluentValidation();
}

var app = builder.Build();
{
	app.SetupRequestPipeline();
	app.MapPostEndpoints();
	app.MapCategoryEndpoints();
	app.MapGameEndpoints();
	app.MapTagEndpoints();
	app.MapSubscriberEndpoints();
	app.MapCommentEndpoints();


	app.Run();
}
