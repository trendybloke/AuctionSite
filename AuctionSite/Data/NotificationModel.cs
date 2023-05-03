using MudBlazor;

namespace AuctionSite.Data
{
	public class NotificationModel
	{
		public int ID { get; set; }
		public string UserID { get; set; }
		public string Content { get; set; }
		public string RedirectURL { get; set; }
		public Severity Severity { get; set; }
	}
}
