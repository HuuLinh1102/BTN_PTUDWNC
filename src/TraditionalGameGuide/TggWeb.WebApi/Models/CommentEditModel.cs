

using TggWeb.Core.Entities;

namespace TggWeb.WebApi.Models
{
	public class CommentEditModel
	{
		public string Content { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsApproved { get; set; }
		public int PostId { get; set; }
		public Post Post { get; set; }
		public int SubscriberId { get; set; }
		public Subscriber Subscriber { get; set; }
	}
}
