﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TggWeb.Core.Entities
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string UrlSlug { get; set; }
		public string Description { get; set; }
		public IList<Game> Games { get; set; }
		public IList<Post> Posts { get; set; }
	}
}
