

namespace TggWeb.WebApi.Models
{
	public class GameEditModel
	{
		public string Name { get; set; }
		public string UrlSlug { get; set; }
		public string Description { get; set; }
		public int Age { get; set; }
		public int PlayerCount { get; set; }
		public int CategoryId { get; set; }
		public CategoryDto Category { get; set; }
	}
}
