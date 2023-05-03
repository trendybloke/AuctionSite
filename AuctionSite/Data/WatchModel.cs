using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSite.Data
{
	[PrimaryKey(nameof(AuctionID), nameof(WatchingUserID))]
	public class WatchModel
	{
		public int AuctionID { get; set; }
		public string WatchingUserID { get; set; }
	}
}
