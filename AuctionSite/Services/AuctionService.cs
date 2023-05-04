using AuctionSite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;

namespace AuctionSite.Services
{
	public class AuctionService
	{
		[Inject]
		IDbContextFactory<ApplicationDbContext> DbContextFactory { get; set; }

		public AuctionService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
		{
			DbContextFactory = dbContextFactory;
		}

		public async Task<AuctionModel?> GetAuctionAsync(int? id)
		{
			AuctionModel? auction;

			using(var context = await DbContextFactory.CreateDbContextAsync())
			{
				auction = await context.Auctions.FindAsync(id);
			}

			return auction;
		}

		public async Task<AuctionModel[]> GetAuctionsAsync()
		{
			AuctionModel[] auctions;

			using(var context = await DbContextFactory.CreateDbContextAsync())
			{
				auctions = context.Auctions.ToArray();
			}

			return auctions;
		}

		public async Task<AuctionModel[]> GetAuctionsByAdminAsync(string adminUserId)
		{
			AuctionModel[] adminAuctions;

			using(var context = await DbContextFactory.CreateDbContextAsync())
			{
				adminAuctions = context.Auctions
										.Where(a => a.CreatorUserID == adminUserId)
										.ToArray();
			}

			return adminAuctions;
		}
	}
}
