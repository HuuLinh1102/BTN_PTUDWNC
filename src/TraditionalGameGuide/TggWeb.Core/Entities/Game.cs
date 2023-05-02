using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TggWeb.Core.Entities
{
	public class Game
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
		public IList<Post> Posts { get; set; }
	}
}
