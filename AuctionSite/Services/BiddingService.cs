using AuctionSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;

namespace AuctionSite.Services
{
	public class BiddingService
	{
		[Inject]
		public IDbContextFactory<ApplicationDbContext>? DbContextFactory { get; set; }

		public BiddingService(IDbContextFactory<ApplicationDbContext>? dbContextFactory)
		{
			DbContextFactory = dbContextFactory;
		}

		/* Client Methods */
		// Called by clients when they are placing a bid. Saves model to the DB.
		public async Task PlaceBidAsync(BidModel newBid)
		{
			using (var context = await DbContextFactory.CreateDbContextAsync())
			{
				await context.Bids.AddAsync(newBid);

				await context.SaveChangesAsync();

				await context.DisposeAsync();
			}
		}

		// Called by clients when they are placing a bid. Saves model to the DB, and notifies a connected bidhub for UI updates.
		public async Task PlaceBidAsync(BidModel newBid, HubConnection bidHubConnection)
		{
			await PlaceBidAsync(newBid);

			await bidHubConnection.SendAsync("BidPlaced", newBid);
		}

		// Called by clients to get the bid history of a given auction or user.
		public BidModel[]? GetBidHistory(AuctionModel auction)
		{
			BidModel[]? bidHistory;

			using (var context = DbContextFactory.CreateDbContext())
			{
				bidHistory = context.Bids
										.Where(b => b.AuctionID == auction.Id)
										.OrderByDescending(b => b.Amount)
										.ToArray();
			}

			return bidHistory;
		}

		public async Task<BidModel[]?> GetBidHistoryAsync(AuctionModel auction)
		{
			BidModel[]? bidHistory;

			using (var context = await DbContextFactory.CreateDbContextAsync())
			{
				bidHistory = context.Bids
										.Where(b => b.AuctionID == auction.Id)
										.OrderByDescending(b => b.Amount)
										.ToArray();
			}

			return bidHistory;
		}

		public async Task<BidModel[]?> GetBidHistoryAsync(string userid)
		{
			BidModel[]? bidHistory;

			using (var context = await DbContextFactory.CreateDbContextAsync())
			{
				bidHistory = context.Bids
										.Where(b => b.UserID == userid)
										.OrderByDescending(b => b.CreatedOn)
										.ToArray();
			}

			return bidHistory;
		}

		// Called by clients when there is a need to find the highest bid on an item
		public async Task<BidModel?> GetHighestBidAsync(AuctionModel auction)
		{
			BidModel? highestBid;

			using (var context = await DbContextFactory.CreateDbContextAsync())
			{
				highestBid = await context.Bids
											.Where(b => b.AuctionID == auction.Id)
											.OrderByDescending(b => b.Amount)
											.FirstOrDefaultAsync();
			}

			return highestBid;
		}

		public BidModel GetHighestBid(AuctionModel auction)
		{
			BidModel? highestBid;

			using (var context = DbContextFactory.CreateDbContext())
			{
				highestBid = context.Bids
										.Where(b => b.AuctionID == auction.Id)
										.OrderByDescending(b => b.Amount)
										.FirstOrDefault();
			}

			return highestBid;
		}

		/* Server Methods */
		// Called by BiddingHub to associate an Auction with a Bid.
		public async Task<AuctionModel?> GetAuctionFromBidAsync(BidModel bid)
		{
			AuctionModel? biddedAuction;

			using (var context = await DbContextFactory.CreateDbContextAsync())
			{
				biddedAuction = context.Auctions
											.Where(a => a.Id == bid.AuctionID)
											.FirstOrDefault();

				await context.DisposeAsync();
			}

			return biddedAuction;
		}
	}
}
