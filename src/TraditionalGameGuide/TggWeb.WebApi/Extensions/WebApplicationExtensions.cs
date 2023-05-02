using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TggWeb.Data.Contexts;
using TggWeb.Services.Webs;
using TggWeb.Services.Media;
using TggWeb.Services.Timing;

namespace TggWeb.WebApi.Extensions
{
	public static class WebApplicationExtensions
	{
		public static WebApplicationBuilder ConfigureServices(
			this WebApplicationBuilder builder)
		{
			builder.Services.AddMemoryCache();

			builder.Services.AddDbContext<WebDbContext>(options =>
				options.UseSqlServer(
				builder.Configuration
						.GetConnectionString("DefaultConnection")));

			builder.Services
				.AddScoped<ITimeProvider, LocalTimeProvider>();
			builder.Services
				.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
			builder.Services
				.AddScoped<IWebRepository, WebRepository>();
			builder.Services
				.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services
				.AddScoped<IGameRepository, GameRepository>();
			builder.Services
				.AddScoped<ITagRepository, TagRepository>();
			builder.Services
				.AddScoped<ISubscriberRepository, SubscriberRepository>();
			builder.Services
				.AddScoped<ICommentRepository, CommentRepository>();


			return builder;
		}

		public static WebApplicationBuilder ConfigureCors(
			this WebApplicationBuilder builder)
		{
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("TggWebApp", policyBuilder =>
					policyBuilder
								.AllowAnyOrigin()
								.AllowAnyHeader()
								.AllowAnyMethod());
			});

			return builder;
		}

		// Cấu hình việc sử dụng NLog
		public static WebApplicationBuilder ConfigureNLog(
			this WebApplicationBuilder builder)
		{
			builder.Logging.ClearProviders();
			builder.Host.UseNLog();

			return builder;
		}

		public static WebApplicationBuilder ConfigureSwaggerOpenApi(
			this WebApplicationBuilder builder)
		{
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			return builder;
		}

		public static WebApplication SetupRequestPipeline(
			this WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseStaticFiles();
			app.UseHttpsRedirection();

			app.UseCors("TggWebApp");

			return app;
		}
	}
}
