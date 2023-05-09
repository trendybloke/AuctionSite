using AuctionSite.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSite.Data
{
	public class AuctionModel
	{
		public int Id { get; set; }

		public string CreatorUserID { get; set; }

		public string Title { get; set; }
		public string Description { get; set; }

		// Price that is initialized as the lowest amount already bid
		public float StartPrice { get; set; }

		//Lowest price that the buyer will accept
		public float ReservePrice { get; set; }

		public ItemCondition Condition { get; set; }
		public AuctionCategory Category { get; set; }
		public AuctionState State { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public List<int> ImageIDs { get; set; }

		//[NotMapped]
		//public List<UploadedFile> Images { get; set; }
	}
}
