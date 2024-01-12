using Common.Core.Classes;

namespace TestHarness;

internal class TestAddress : DataFactoryBase
{
	internal static string[] postcodes = [
		"48908",      // Katheryn A Tanguma
		"46204-3561", // Buford Cocopoti
		"98499-2666", // Margaret Crumm
		"80112",      // Bens Iron Works
		"80904",      // Internal Medicine Clinic
		"21117"       // Mega Allied Services
		];

	internal static bool RunTest( AddrParams args )
	{
		switch( AddressFactoryBase.Countries )
		{
			case not null when AddressFactoryBase.Countries.Count > 0:
				_ = Program.sLogger.Info( $"Countries count.: {AddressFactoryBase.Countries.Count:00#}" );
				break;
			default:
				_ = Program.sLogger.Info( "No Countries loaded!" );
				return false;
		}

		switch( AddressFactoryBase.Provinces )
		{
			case not null when AddressFactoryBase.Provinces.Count > 0:
				_ = Program.sLogger.Info( $"Provinces count.: {AddressFactoryBase.Provinces.Count:00#}" );
				break;
			default:
				_ = Program.sLogger.Info( "No Provinces loaded!" );
				return false;
		}

		switch( AddressFactoryBase.PostcodeCount )
		{
			case > 0:
				_ = Program.sLogger.Info( $"Postcodes count.: {AddressFactoryBase.PostcodeCount:#,00#}" );
				break;
			default:
				_ = Program.sLogger.Info( "No Postcodes loaded!" );
				return false;
		}

		Address address = new() { Country = AddressFactoryBase.DefaultCountry };

		// Pick a random Province
		var province = AddressFactoryBase.Provinces[sRandom.Next( AddressFactoryBase.Provinces.Count - 1 )];
		address.Province = province.Code;

		// Pick a random County
		if( args.Counties.Count == 0 )
		{
			_ = Program.sLogger.Info( "No counties supplied!" );
			return false;
		}
		address.County = args.Counties[sRandom.Next( args.Counties.Count - 1 )];

		// Pick a random City
		if( args.Cities.Count == 0 )
		{
			_ = Program.sLogger.Info( "No cities supplied!" );
			return false;
		}
		address.County = args.Cities[sRandom.Next( args.Cities.Count - 1 )];

		// Pick a random Postcode
		if( args.Postcodes.Count == 0 )
		{
			_ = Program.sLogger.Info( "No postcodes supplied!" );
			return false;
		}
		address.Postcode = args.Postcodes[sRandom.Next( args.Postcodes.Count - 1 )];

		return true;
	}

	private class Address : Common.Core.Models.Address
	{
		public string? County { get; set; }
	}
}

internal class AddrParams
{
	internal List<string?> Counties { get; set; } = new();

	internal List<string?> Cities { get; set; } = new ();

	internal List<string?> Postcodes { get; set; } = new();
}