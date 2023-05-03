using System.ComponentModel.DataAnnotations;

namespace AuctionSite.Views
{
	public class LoginUser
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
