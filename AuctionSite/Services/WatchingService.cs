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
	}
}
