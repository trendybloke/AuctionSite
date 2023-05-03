using AuctionSite.Data;

namespace AuctionSite.Views
{
	public class UserSummary
	{
		public string ID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Name => $"{FirstName} {LastName}";
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public float Balance { get; set; }

		public UserSummary(ApplicationUser user) 
		{
			ID = user.Id;
			Email = user.Email;
			FirstName = user.FirstName;
			LastName = user.LastName;
			PhoneNumber = user.PhoneNumber;
			Balance = user.Balance;
		}
	}
}
