using MudBlazor;
using AuctionSite.Data;
using AuctionSite.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;

namespace AuctionSite.Hubs
{
	/// <summary>
	/// 
	///		Signals sent: 
	///			- Update-Bids
	///				Occurs when someone has placed a bid. Should tell any page reliant on bids that they should refresh their collections.
	/// 
	/// </summary>
	public class BiddingHub : Hub
	{
		[Inject]
		public BiddingService BiddingService { get; set; }

		[Inject]
		public WatchingService WatchingService { get; set; }
		
		[Inject]
		public NotificationService NotificationService { get; set; }

		public BiddingHub(BiddingService biddingService, WatchingService watchingService)
		{
			BiddingService = biddingService;
			WatchingService = watchingService;
		}

		public async Task BidPlaced(BidModel bid)
		{
			AuctionModel? biddedAuction = await BiddingService.GetAuctionFromBidAsync(bid);

			string notifContent = $"Someone placed a bid for {biddedAuction.Title}";
			Severity notifSeverity = Severity.Info;

			string[] watchingUserIds = await WatchingService.GetUsersWatchingAuctionAsync(biddedAuction);

			//foreach(string userid in biddedAuction.WatchingUserIDs)
			foreach(string userid in watchingUserIds)		// Possible refactor?
			{
				// Notify user
				NotificationModel newNotif = new NotificationModel()
				{
					UserID = userid,
					Content = notifContent,
					Severity = notifSeverity,
					RedirectURL = $"/auction/{biddedAuction.Id}"
				};

				await Clients.User(userid).SendAsync("NewNotification", newNotif);
				
				await NotificationService.PersistNotificationAsync(newNotif);
			}

			await Clients.All.SendAsync("Update-Bids");
		}

	}
}
