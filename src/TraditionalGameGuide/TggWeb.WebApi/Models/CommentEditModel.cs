

using Microsoft.AspNetCore.Mvc.Rendering;
using TggWeb.Core.Entities;

namespace TggWeb.WebApi.Models
{
	public class CommentEditModel
	{
		public string Content { get; set; }
		public bool IsApproved { get; set; }
		public int PostId { get; set; }
		public IEnumerable<SelectListItem> Post { get; set; }
		public int SubscriberId { get; set; }
		public IEnumerable<SelectListItem> Subscriber { get; set; }
	}
}
