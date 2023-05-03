using AuctionSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MudBlazor;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace AuctionSite.Pages
{
	public class PageBase : ComponentBase // , IDisposable
	{
		[Inject]
		public IDbContextFactory<ApplicationDbContext>? DbContextFactory { get; set; }

		[Inject]
		public NavigationManager NavigationManager { get; set; }

		[Inject]
		public IJSRuntime JS { get; set; }

		[Inject]
		public ISnackbar Snackbar { get; set; }

		//protected async override Task OnInitializedAsync()
		//{
		//	await base.OnInitializedAsync();
		//}

		[CascadingParameter]
		protected Task<AuthenticationState> AuthenticationStateTask { get; set; }

		protected ClaimsPrincipal? CurrentUser
		{
			get
			{
				var authState = AuthenticationStateTask.Result;
				var user = authState.User;
				return user;
			}
		}
	}
}
