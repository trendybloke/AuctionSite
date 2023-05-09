using AuctionSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AuctionSite.Services
{
	public class ImageService
	{
		[Inject]
		IDbContextFactory<ApplicationDbContext> DbContextFactory { get; set; }

		public ImageService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
		{
			DbContextFactory = dbContextFactory;
		}

		public async Task<int> UploadImageAsync(UploadedFile image)
		{
			int imageId;

			using (var context = await DbContextFactory.CreateDbContextAsync())
			{
				await context.Images.AddAsync(image);

				var saveResult = await context.SaveChangesAsync();

				if (saveResult == 0)
					imageId = -1;
				else
					imageId = image.Id;
			}

			return image.Id;
		}

		public UploadedFile? GetPrimaryImage(AuctionModel auction)
		{
			UploadedFile? image;

			using (var context = DbContextFactory.CreateDbContext())
			{
				if (auction.ImageIDs is not null && auction.ImageIDs.Count > 0)
					image = context.Images.Where(i => i.Id == auction.ImageIDs[0]).FirstOrDefault();
				else
					image = null;
			}

			return image;
		}

		public async Task<UploadedFile?> GetPrimaryImageAsync(AuctionModel auction)
		{
			UploadedFile? image;

			using (var context = await DbContextFactory.CreateDbContextAsync())
			{
				if(auction.ImageIDs is not null && auction.ImageIDs.Count > 0)
					image = await context.Images.Where(i => i.Id == auction.ImageIDs[0]).FirstOrDefaultAsync();
				else
					image = null;
			}

			return image;
		}
	}
}
