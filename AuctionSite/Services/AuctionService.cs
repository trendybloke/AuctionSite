using AuctionSite.Data;
using AuctionSite.Enums;
using System.Timers;
//using AuctionSite.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

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

		public async Task<UploadedFile[]> GetAuctionImagesAsync(int? id)
		{
			List<UploadedFile> auctionImages = new List<UploadedFile>();

			using(var context = await DbContextFactory.CreateDbContextAsync())
			{
				var auction = await context.Auctions.FindAsync(id);

				foreach(var imageId in auction.ImageIDs)
				{
					var image = await context.Images.FindAsync(imageId);

					auctionImages.Add(image);
				}
			}

			return auctionImages.ToArray();
		}

		public async Task<UploadedFile[]> GetAuctionImagesAsync(AuctionModel auction)
		{
			return await GetAuctionImagesAsync(auction.Id);
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

		public async Task<bool> CreateAuctionAsync(AuctionModel auction)
		{
			int saveResult;

			using(var context = await DbContextFactory.CreateDbContextAsync())
			{
				context.Auctions.Add(auction);

				saveResult = await context.SaveChangesAsync();
			}

			return saveResult > 0;
		}

		public async Task<bool> EditAuctionAsync(AuctionModel newAuction)
		{
			int saveResult;

			using (var context = await DbContextFactory.CreateDbContextAsync())
			{
				var oldAuction = context.Auctions.Find(newAuction.Id);

				oldAuction.Title = newAuction.Title;
				oldAuction.Description = newAuction.Description;
				oldAuction.StartPrice = newAuction.StartPrice;
				oldAuction.ReservePrice = newAuction.ReservePrice;
				oldAuction.Condition = newAuction.Condition;
				oldAuction.State = newAuction.State;
				oldAuction.StartDate = newAuction.StartDate;
				oldAuction.EndDate = newAuction.EndDate;
				oldAuction.ImageIDs = newAuction.ImageIDs;

				saveResult = await context.SaveChangesAsync();
			}

			return saveResult > 0;
		}

		public async Task<bool> DeleteAuctionAsync(AuctionModel auction)
		{
			int delResult;
			using(var context = await DbContextFactory.CreateDbContextAsync())
			{
				int bidsOnAuction = context.Bids.Where(b => b.AuctionID == auction.Id).Count();

				if(bidsOnAuction > 0)
				{
					// Someone has bid on this auction, therefore it cannot be deleted
					return false;
				}

				context.Auctions.Remove(auction);

				delResult = await context.SaveChangesAsync();
			}

			return delResult > 0;
		}
	}
}
