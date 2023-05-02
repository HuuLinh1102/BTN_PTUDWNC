using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TggWeb.Core.Entities;

namespace TggWeb.Core.DTO
{
	public class CommentItem
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsApproved { get; set; }
		public int PostId { get; set; }
		public Post Post { get; set; }
		public int SubscriberId { get; set; }
		public Subscriber Subscriber { get; set; }
	}
}
