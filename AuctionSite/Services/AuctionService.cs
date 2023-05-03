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

		public async Task<AuctionModel[]> GetAuctionsByAdmin(string adminUserId)
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
