namespace AuctionSite.Enums
{
	public enum AuctionCategory
	{
		// Examples for a category are stated as "Desktop PC", "Laptop", "Monitor", "Complete system (desktop with monitor)", "Graphics card"
		
		// Categories:
			// Complete system 		- PC and monitor, Laptop, etc.
			// Incomplete system		- Just a PC
			// Faulty system 		- Can be complete or incomplete, but is broken to some degree
			// System component		- Graphics card, processor, etc.
			
		// BitField can be used to imply multiple categories;
			// A Complete and Faulty system
			// An incomplete system consisting of a series of packaged components
	}
}
