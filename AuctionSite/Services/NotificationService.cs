using AuctionSite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;

namespace AuctionSite.Services
{
	public class NotificationService
	{
		[Inject]
		IDbContextFactory<ApplicationDbContext>? DbContextFactory { get; set; }

		public NotificationService(IDbContextFactory<ApplicationDbContext> dbContextFactory) 
		{
			DbContextFactory = dbContextFactory;
		}

		public async Task<bool> PersistNotificationAsync(NotificationModel notificationModel)
		{
			int saveResult; 

			using (var context = await DbContextFactory.CreateDbContextAsync())
			{
				context.Notifications.Add(notificationModel);

				saveResult = await context.SaveChangesAsync();

				await context.DisposeAsync();
			}

			return saveResult > 0;
		}

		public async Task<bool> PersistMultipleNotificationsAsync(NotificationModel[] notificationModels)
		{
			int saveResult = 0;

			foreach (var notificationModel in notificationModels)
			{
				bool thisSave = await PersistNotificationAsync(notificationModel);
				if(thisSave)
					saveResult++;
			}

			return saveResult == notificationModels.Length;
		}
	}
}
