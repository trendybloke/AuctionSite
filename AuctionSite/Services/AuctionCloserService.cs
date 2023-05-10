using AuctionSite.Data;
using AuctionSite.Enums;
using AuctionSite.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Timers;

namespace AuctionSite.Services
{
	public class AuctionCloseEventArgs
	{
		public int ClosedAuctionID { get; }
		public AuctionCloseEventArgs(int auctionID)
		{
			ClosedAuctionID = auctionID;
		}
	}

	public class AuctionCloserService
	{
		[Inject]
		IDbContextFactory<ApplicationDbContext> DbContextFactory { get; set; }

		private System.Timers.Timer NextToEndTimer;
		private AuctionModel? NextToEnd;
		private readonly TimeSpan _nextToEndInterval = TimeSpan.FromSeconds(1);

		public delegate void OnAuctionEndEventHandler(object sender, AuctionCloseEventArgs e);

		public event OnAuctionEndEventHandler OnAuctionEnd;

		public AuctionCloserService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
		{
			DbContextFactory = dbContextFactory;

			// Determine next to end
			NextToEnd = FindNextAuctionToEnd();

			NextToEndTimer = new(_nextToEndInterval);

			NextToEndTimer.Elapsed += NextToEndTimerElapsedAsync;

			NextToEndTimer.Start();

			//// Build hub connection
			//auctionHub = new HubConnectionBuilder()
			//					.WithUrl("/AuctionHub")
			//					.Build();

			//auctionHub.StartAsync().RunSynchronously();
		}
		private async void NextToEndTimerElapsedAsync(object sender, ElapsedEventArgs e)
		{
			// Find the next auction to end
			NextToEnd = FindNextAuctionToEnd();

			// Has next to end ended? Return if not
			if (NextToEnd.EndDate.CompareTo(DateTime.Now) == 1)
				return;

			// Make sure that next to end is closed
			NextToEnd.State = AuctionState.Closed;

			using (var context = await DbContextFactory.CreateDbContextAsync())
			{
				context.Auctions.Update(NextToEnd);

				var saveResult = await context.SaveChangesAsync();

				if (saveResult == 0)
					throw new Exception("Couldn't overwrite NextToEnd");
			}

			// Signal all clients to update
			OnAuctionEnd.Invoke(this, new AuctionCloseEventArgs(NextToEnd.Id));
			//await Clients.All.SendAsync("Auction-Ended");
		}

		private AuctionModel? FindNextAuctionToEnd()
		{
			AuctionModel? auction = null;

			using (var context = DbContextFactory.CreateDbContext())
			{
				auction = context.Auctions
									.Where(a => a.State == AuctionState.Open)
									.OrderBy(a => a.EndDate).FirstOrDefault();
			}

			return auction;
		}
	}
}
