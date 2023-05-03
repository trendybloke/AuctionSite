using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using AuctionSite.Data;
using System.Security.Claims;
using System.Runtime.CompilerServices;

namespace AuctionSite.Services
{
	public class ClaimsPrincipalFactory :
		UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
	{
		public ClaimsPrincipalFactory(
				UserManager<ApplicationUser> userManager,
				RoleManager<IdentityRole> roleManager,
				IOptions<IdentityOptions> optionsAccessor
			) : base(userManager, roleManager, optionsAccessor) { }

		public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
		{
			var principal = await base.CreateAsync(user);

			if (!string.IsNullOrEmpty(user.FirstName))
			{
				if (principal.Identity != null)
				{
					((ClaimsIdentity)principal.Identity).AddClaim(
						new Claim(ClaimTypes.GivenName, user.FirstName));
				}

			}

			if (!string.IsNullOrEmpty(user.LastName))
			{
				if (principal.Identity != null)
				{
					((ClaimsIdentity)principal.Identity).AddClaim(
						new Claim(ClaimTypes.Surname, user.LastName));
				}
			}

			return principal;
		}
	}
}
