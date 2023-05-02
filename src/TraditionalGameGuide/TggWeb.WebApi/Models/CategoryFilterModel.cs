namespace TggWeb.WebApi.Models
{
	public class CategoryFilterModel : PagingModel
	{
		public string Name { get; set; }

		public string UrlSlug { get; set; }

	}
}
