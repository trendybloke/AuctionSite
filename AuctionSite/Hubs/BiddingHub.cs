using MudBlazor;
using AuctionSite.Data;
using AuctionSite.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace AuctionSite.Hubs
{
	public class BiddingHub : Hub
	{
		//[Inject]
		//public IDbContextFactory<ApplicationDbContext> DbContextFactory /*{ get; set; } = default!*/;

		[Inject]
		public BiddingService BiddingService { get; set; }

		[Inject]
		public WatchingService WatchingService { get; set; }

		//public BiddingHub(IDbContextFactory<ApplicationDbContext> dbContextFactory)
		//{
		//	this.DbContextFactory = dbContextFactory;
		//}

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
			foreach(string userid in watchingUserIds)
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
			}

			// Get all watching users
			//using (var context = DbContextFactory.CreateDbContext())
			//{
			//	foreach (string userid in bid.Auction.WatchingUserIDs)
			//	{
			//		// Add notification for them
			//		NotificationModel newNotif = new NotificationModel()
			//		{
			//			UserID = userid,
			//			Content = notifContent,
			//			Severity = notifSeverity,
			//			RedirectURL = $"/auction/{bid.AuctionID}"
			//		};

			//		// Send notif update
			//		await Clients.User(userid).SendAsync("NewNotification", newNotif);

			//		// Save notif
			//		context.Notifications.Add(newNotif);
			//	}

			//	await context.SaveChangesAsync();
			//}
		}

		//public async Task PlaceBid(BidModel bid)
		//{
		//	string notifContent = $"Someone placed a bid for {bid.Auction.Title}";
		//	Severity notifSeverity = Severity.Info;

		//	using (var context = DbContextFactory.CreateDbContext())
		//	{
		//		// Save bid to db
		//		context.Bids.Add(bid);

		//		var saveResult = await context.SaveChangesAsync();


		//		if (saveResult < 0)
		//		{
		//			// Failed to save, alert the user
		//			await Clients.User(bid.UserID).SendAsync("NewNotification", new NotificationModel()
		//			{
		//				Content = $"Failed to bid on {bid.Auction.Title}",
		//				Severity = Severity.Error,
		//				UserID = bid.UserID,
		//				RedirectURL = $"/auction/{bid.AuctionID}"
		//			});
		//		}
		//	}

		//	// Ensure that this user is watching this auction
		//	using(var context = DbContextFactory.CreateDbContext())
		//	{
		//		var thisAuction = context.Auctions.Find(bid.AuctionID);

		//		context.Auctions.Attach(thisAuction);

		//		if (!thisAuction.WatchingUserIDs.Contains(bid.UserID))
		//		{
		//			thisAuction.WatchingUserIDs.Add(bid.UserID);
		//		}

		//		await context.SaveChangesAsync();
		//	}

		//	// Get all watching users
		//	using(var context = DbContextFactory.CreateDbContext())
		//	{
		//		foreach(string userid in bid.Auction.WatchingUserIDs)
		//		{
		//			// Add notification for them
		//			NotificationModel newNotif = new NotificationModel()
		//			{
		//				UserID = userid,
		//				Content = notifContent,
		//				Severity = notifSeverity,
		//				RedirectURL = $"/auction/{bid.AuctionID}"
		//			};

		//			// Send notif update
		//			await Clients.User(userid).SendAsync("NewNotification", newNotif);

		//			// Save notif
		//			context.Notifications.Add(newNotif);
		//		}

		//		await context.SaveChangesAsync();
		//	}

		//	//}
		//}

		public async Task WatchAuction(AuctionModel auction, string userid)
		{
			//using(var context = DbContextFactory.CreateDbContext())
			//{
				
			//}
		}
	}
}
