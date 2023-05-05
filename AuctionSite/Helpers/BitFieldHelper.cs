using AuctionSite.Enums;

namespace AuctionSite.Helpers
{
	public static class BitFieldHelper
	{
		public static string ToFormattedString(this AuctionCategory categoryField)
		{
			return categoryField.ToString("f").Replace('_', ' ');
		}
	}
}
