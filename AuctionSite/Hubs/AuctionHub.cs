using AuctionSite.Data;
using AuctionSite.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using System.Security.Cryptography;

namespace AuctionSite.Hubs
{
	public class AuctionHub : Hub
	{
		[Inject]
		public AuctionCloserService AuctionCloserService { get; set; }

		[Inject]
		public NotificationService NotificationService { get; set; }

		[Inject]
		public WatchingService WatchingService { get; set; }

		public AuctionHub(AuctionCloserService auctionCloserService, NotificationService notificationService, WatchingService watchingService)
		{
			AuctionCloserService = auctionCloserService;
			NotificationService = notificationService;
			WatchingService = watchingService;
			//AuctionCloserService.OnAuctionEnd += BroadcastAuctionEnded;
		}

		//async void BroadcastAuctionEnded(object sender, AuctionCloseEventArgs e)
		//{
		//	await Clients.All.SendAsync("Auction-Ended");
		//}

		public async Task AuctionEnded(AuctionModel endedAuction)
		{
            string notifContent = $"{endedAuction.Title} has ended!";
            Severity notifSeverity = Severity.Info;

            // Build and persist notifications
            string[] watchingUserIds = await WatchingService.GetUsersWatchingAuctionAsync(endedAuction);

			foreach (string userid in watchingUserIds)
			{
				// Notify user
				NotificationModel newNotif = new NotificationModel()
				{
					UserID = userid,
					Content = notifContent,
					Severity = notifSeverity,
					RedirectURL = $"/auction/{endedAuction.Id}"
				};

				//await Clients.User(userid).SendAsync("NewNotification");
				var saveResult = await NotificationService.PersistNotificationAsync(newNotif);
			}

            // Send signal
            await Clients.All.SendAsync("Auction-Ended");
		}

		//public async Task AuctionEnded(AuctionModel auction)
		//{
		//	await Clients.All.SendAsync("Auction-Ended", auction);
		//}
	}
}
