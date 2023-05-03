using AuctionSite.Views;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSite.Data
{
	[PrimaryKey(nameof(AuctionID), nameof(UserID), nameof(Amount))]
	public class BidModel
	{
		public int AuctionID { get; set; }
		public string UserID { get; set; }
		public float Amount { get; set; }

		public DateTime CreatedOn { get; set; }

		[NotMapped]
		public AuctionModel? Auction { get; set; }
	}
}
