using System;
using TggWeb.Core.Entities;

namespace TggWeb.WebApi.Models
{
	public class CommentDto
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsApproved { get; set; }
		public int PostId { get; set; }
		public int SubscriberId { get; set; }
	}
}
