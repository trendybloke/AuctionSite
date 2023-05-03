using Microsoft.AspNetCore.Identity;

namespace AuctionSite.Data
{
	public class ApplicationUser : IdentityUser
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? PhoneNumber { get; set; }
		public float Balance { get; set; }
	}
}
