

namespace TggWeb.WebApi.Models
{
	public class SubscriberEditModel
	{
		public string Email { get; set; }
		public string UnsubscribeReason { get; set; }
		public bool IsUserInitiatedUnsubscribe { get; set; } = false;
		public string AdminNote { get; set; }
	}
}
