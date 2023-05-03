using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuctionSite.Data;

namespace AuctionSite.Controllers
{
	public class AccountController : Controller
	{
		private readonly IDataProtectionProvider _dataProtectionProvider;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(
				IDataProtectionProvider dataProtectionProvider, 
				UserManager<ApplicationUser> userManager, 
				SignInManager<ApplicationUser> signInManager
			)
		{
			_dataProtectionProvider = dataProtectionProvider;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpGet("/Account/LoginInternal")]
		[AllowAnonymous]
		public async Task<IActionResult> LoginInternal(string token)
		{
			var dataProtector = _dataProtectionProvider.CreateProtector("LoginInternal");
			var data = dataProtector.Unprotect(token);
			var parts = data.Split('|');
			var identityUser = await _userManager.FindByIdAsync(parts[0]);

			if(identityUser == null)
			{
				return Unauthorized();
			}

			var isTokenValid = await _userManager.VerifyUserTokenAsync(identityUser, TokenOptions.DefaultProvider, "LoginInternal", parts[1]);

			if(isTokenValid)
			{
				var isPersistent = bool.Parse(parts[2]);
				await _userManager.ResetAccessFailedCountAsync(identityUser);
				await _signInManager.SignInAsync(identityUser, isPersistent);
				return Redirect(parts[3]);
			}

			return Unauthorized();
		}

		[HttpPost("/Account/Logout")]
		[AllowAnonymous]
		public async Task<IActionResult> Logout()
		{
			if (_signInManager.IsSignedIn(User))
			{
				await _signInManager.SignOutAsync();
			}
			return Redirect("~/");
		}
	}
}
