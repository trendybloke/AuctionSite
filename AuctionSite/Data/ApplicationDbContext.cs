using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using AuctionSite.Enums;

namespace AuctionSite.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<AuctionModel> Auctions { get; set; }
		public DbSet<BidModel> Bids { get; set; }
		public DbSet<NotificationModel> Notifications { get; set; }
		public DbSet<WatchModel> Watching { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if(!optionsBuilder.IsConfigured)
			{
				IConfiguration Configuration = new ConfigurationBuilder()
					.AddJsonFile("appsettings.json")
					.AddEnvironmentVariables()
					.Build();

				optionsBuilder
					.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
					.EnableSensitiveDataLogging()
					.LogTo(x => System.Diagnostics.Debug.WriteLine(x));
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			//builder.Entity<AuctionModel>()
			//	.Property(am => am.WatchingUserIDs)
			//	.HasConversion(
			//		u => JsonConvert.SerializeObject(u),
			//		u => JsonConvert.DeserializeObject<List<string>>(u)
			//	);

			// Newtonsoft conversions
			builder.Entity<AuctionModel>()
				.Property(am => am.ImageIDs)
				.HasConversion(
					i => JsonConvert.SerializeObject(i),
					i => JsonConvert.DeserializeObject<List<string>>(i)
				);

			builder.Entity<AuctionModel>()
				.HasData(new AuctionModel[]
				{
					new AuctionModel
					{
						Id = 1,
						CreatorUserID = "97faa063-c39d-4f5d-aa60-71f7cc6a0028",
						Title = "Test Auction A",
						Description = "A test auction.",
						StartPrice = 15.50f,
						ReservePrice = 0f,
						Condition = ItemCondition.Average,
						State = AuctionState.Open,
						StartDate = new DateTime(2023, 1, 1),
						EndDate = new DateTime(2024, 1, 1),
						//WatchingUserIDs = new List<string>(),
						ImageIDs = new List<string>()
					},
					new AuctionModel
					{
						Id = 2,
						CreatorUserID = "97faa063-c39d-4f5d-aa60-71f7cc6a0028",
						Title = "Test Auction B",
						Description = "A test auction.",
						StartPrice = 5.25f,
						ReservePrice = 0f,
						Condition = ItemCondition.Poor,
						State = AuctionState.Open,
						StartDate = new DateTime(2023, 1, 1),
						EndDate = new DateTime(2024, 1, 1),
						//WatchingUserIDs = new List<string>(),
						ImageIDs = new List<string>()
					},
					new AuctionModel
					{
						Id = 3,
						CreatorUserID = "97faa063-c39d-4f5d-aa60-71f7cc6a0028",
						Title = "Test Auction C",
						Description = "A test auction.",
						StartPrice = 225.0f,
						ReservePrice = 250.0f,
						Condition = ItemCondition.New,
						State = AuctionState.Open,
						StartDate = new DateTime(2023, 1, 1),
						EndDate = new DateTime(2024, 1, 1),
						//WatchingUserIDs = new List<string>(),
						ImageIDs = new List<string>()
					}
				});
		}
	}
}
