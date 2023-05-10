using AuctionSite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;

namespace AuctionSite.Services
{
	public class WatchingService
	{
		[Inject]
		IDbContextFactory<ApplicationDbContext>? DbContextFactory { get; set; }

		public WatchingService(IDbContextFactory<ApplicationDbContext>? dbContextFactory) 
		{
			DbContextFactory = dbContextFactory;
		}

		public async Task<bool> StartWatchingAuctionAsync(WatchModel watchModel)
		{
			try
			{
				using(var context = await DbContextFactory.CreateDbContextAsync())
				{
					context.Watching.Add(watchModel);

					await context.SaveChangesAsync();

					await context.DisposeAsync();
				}

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<bool> StopWatchingAuctionAsync(WatchModel watchModel)
		{
			try
			{
				using(var context = await DbContextFactory.CreateDbContextAsync())
				{
					context.Watching.Remove(watchModel);

					await context.SaveChangesAsync();

					await context.DisposeAsync();
				}

				return true;
			}
			catch(Exception ex)
			{
				return false;
			}
		}

		public async Task<string[]> GetUsersWatchingAuctionAsync(AuctionModel auction)
		{
			List<string> userids = new List<string>();

			using (var context = await DbContextFactory.CreateDbContextAsync())
			{
				var watchModels = context.Watching.Where(w => w.AuctionID == auction.Id).ToArray();

				foreach(var model in watchModels)
				{
					userids.Add(model.WatchingUserID);
				}
			}

			return userids.ToArray();
		}

		public AuctionModel[] GetWatchedAuctions(string userid)
		{
			List<AuctionModel> watchedAuctions = new List<AuctionModel>();

			using (var context = DbContextFactory.CreateDbContext())
			{
				WatchModel[] watchModels;

				watchModels = context.Watching.Where(w => w.WatchingUserID == userid).ToArray();

				foreach (var watchModel in watchModels)
				{
					var watchedAuction = context.Auctions.Find(watchModel.AuctionID);

					watchedAuctions.Add(watchedAuction);
				}
			}

			return watchedAuctions.ToArray();
		}

		public async Task<AuctionModel[]> GetWatchedAuctionsAsync(string userid)
		{
			List<AuctionModel> watchedAuctions = new List<AuctionModel>();

			using (var context = await DbContextFactory.CreateDbContextAsync())
			{
				WatchModel[] watchModels;

				watchModels = context.Watching.Where(w => w.WatchingUserID == userid).ToArray();

				foreach(var watchModel in watchModels)
				{
					var watchedAuction = context.Auctions.Find(watchModel.AuctionID);

					watchedAuctions.Add(watchedAuction);
				}
			}

			return watchedAuctions.ToArray();
		}

		public async Task<WatchModel?> GetUserWatchingAuctionAsync(string userId, int auctionId)
		{
			WatchModel? watchModel = null;

			using (var context = await DbContextFactory.CreateDbContextAsync())
			{
				watchModel = await context.Watching
											.Where(w => w.WatchingUserID == userId && w.AuctionID == auctionId)
											.FirstOrDefaultAsync();
			}

			return watchModel;
		} 
	}
}
