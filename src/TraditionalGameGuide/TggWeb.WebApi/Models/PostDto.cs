using System;
using System.Collections.Generic;
using TggWeb.Core.Entities;

namespace TggWeb.WebApi.Models
{
	public class PostDto
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string ShortDescription { get; set; }

		public string Description { get; set; }

		public string UrlSlug { get; set; }

		public string ImageUrl { get; set; }

		public int ViewCount { get; set; }

		public DateTime PostedDate { get; set; }

		public GameDto Game { get; set; }

		public IList<TagDto> Tags { get; set; }

		public IList<CommentDto> Comments { get; set; }
	}
}
