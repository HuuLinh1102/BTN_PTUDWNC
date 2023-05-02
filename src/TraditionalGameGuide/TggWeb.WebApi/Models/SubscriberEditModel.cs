

namespace TggWeb.WebApi.Models
{
	public class SubscriberEditModel
	{
		public string Email { get; set; }
		public DateTime SubscriptionDate { get; set; }
		public DateTime? UnsubscribeDate { get; set; }
		public string UnsubscribeReason { get; set; }
		public bool IsUserInitiatedUnsubscribe { get; set; }
		public string AdminNote { get; set; }
	}
}
