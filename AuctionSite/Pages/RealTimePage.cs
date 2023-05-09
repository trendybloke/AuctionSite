using AuctionSite.Data;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;

namespace AuctionSite.Pages
{
	public class RealTimePage : PageBase, IDisposable
	{
		Timer? timer;

		protected HubConnection? bidHubConnection;

		protected async override Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			// timer initialization
			timer = new Timer(TimerTick, new AutoResetEvent(true), 1000, 1000);

			// bid hub connection
			bidHubConnection = new HubConnectionBuilder()
				.WithUrl(NavigationManager.ToAbsoluteUri("/BiddingHub"))
				.Build();
		}

		protected virtual async Task PlaceBid(BidModel bidModel)
		{
			if(bidHubConnection is not null)
			{
				await bidHubConnection.SendAsync("PlaceBid", bidModel);
			}
		}

		protected virtual void TimerTick(object? stateInfo)
		{
			InvokeAsync(StateHasChanged);
		}

		public void Dispose()
		{
			if(timer is not null)
			{
				timer.Dispose();
			}
		}

		public async ValueTask DisposeAsync()
		{
			if(bidHubConnection is not null )
			{
				await bidHubConnection.DisposeAsync();
			}

			if(timer is not null)
			{
				await timer.DisposeAsync();
			}
		}
	}
}
