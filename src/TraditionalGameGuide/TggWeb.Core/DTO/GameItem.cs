using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TggWeb.Core.Entities;

namespace TggWeb.Core.DTO
{
	public class GameItem
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string UrlSlug { get; set; }
		public string Description { get; set; }
		public string ImageUrl { get; set; }
		public int Age { get; set; }
		public int PlayerCount { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; }
		public int PostCount { get; set; }
	}
}
