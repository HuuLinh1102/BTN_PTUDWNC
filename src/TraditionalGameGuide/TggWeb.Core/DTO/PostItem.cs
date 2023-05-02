﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TggWeb.Core.Entities;

namespace TggWeb.Core.DTO
{
	public class PostItem
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string ShortDescription { get; set; }
		public string Description { get; set; }
		public string UrlSlug { get; set; }
		public string ImageUrl { get; set; }
		public int ViewCount { get; set; }
		public bool Published { get; set; }
		public DateTime PostedDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public int GameId { get; set; }
		public int PostCount { get; set; }
		public Game Game { get; set; }
		public IList<Tag> Tags { get; set; }
		public IList<Comment> Comments { get; set; }
	}
}
