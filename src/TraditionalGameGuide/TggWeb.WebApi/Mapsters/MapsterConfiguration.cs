using Mapster;
using TggWeb.Core.DTO;
using TggWeb.Core.Entities;
using TggWeb.WebApi.Models;

namespace TggWeb.WebApi.Mapsters
{
	public class MapsterConfiguration : IRegister
	{
		public void Register(TypeAdapterConfig config) 
		{
			config.NewConfig<Game, GameDto>();
			config.NewConfig<Game, GameItem>()
				.Map(dest => dest.PostCount,
					src => src.Posts == null ? 0 : src.Posts.Count);

			config.NewConfig<GameEditModel, Game>();


			config.NewConfig<Category, CategoryDto>();
			config.NewConfig<Category, CategoryItem>()
				.Map(dest => dest.PostCount,
					src => src.Games.SelectMany(g => g.Posts) == null ? 0 : src.Games.SelectMany(g => g.Posts).Count())
				.Map(dest => dest.GameCount,
					src => src.Games == null ? 0 : src.Games.Count);

			config.NewConfig<CategoryEditModel, Category>();


			config.NewConfig<Post, PostDto>();
			config.NewConfig<Post, PostDetail>();


			config.NewConfig<Tag, TagDto>();
			config.NewConfig<Tag, TagItem>()
				.Map(dest => dest.PostCount,
					src => src.Posts == null ? 0 : src.Posts.Count);

			config.NewConfig<TagEditModel, Tag>();


			config.NewConfig<Subscriber, SubscriberDto>();
			config.NewConfig<Subscriber, SubscriberItem>();

			config.NewConfig<SubscriberEditModel, Tag>();


			config.NewConfig<Comment, CommentDto>();
			config.NewConfig<Comment, CommentItem>();

			config.NewConfig<CommentEditModel, Tag>();

		}
	}
}
