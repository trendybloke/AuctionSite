using System.Text;

namespace AuctionSite.Helpers
{
	public static class TimeSpanHelper
	{
		public static string GetRemainingTime(this TimeSpan elapsedTime)
		{
			StringBuilder sb = new StringBuilder();

			// Days
			sb.Append(elapsedTime.Days > 0 ? $"{elapsedTime.Days} day" : "");
			sb.Append(elapsedTime.Days > 1 ? "s " : " ");

			// Hours
			sb.Append(elapsedTime.Hours > 0 ? $"{elapsedTime.Hours} hour" : "");
			sb.Append(elapsedTime.Hours > 1 ? "s " : " ");

			// Minutes
			sb.Append(elapsedTime.Minutes > 0 ? $"{elapsedTime.Minutes} minute" : "");
			sb.Append(elapsedTime.Minutes > 1 ? "s " : " ");

			// Seconds
			sb.Append(elapsedTime.Seconds > 0 ? $"{elapsedTime.Seconds} second" : "");
			sb.Append(elapsedTime.Seconds > 1 ? "s " : " ");

			return sb.ToString();
		}
	}
}
