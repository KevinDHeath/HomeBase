using Common.Core.Classes;
using Common.Core.Models;

namespace TestHarness;

internal class TestAddress
{
	internal static string[] postcodes = [
		"48908",      // Katheryn A Tanguma
		"46204-3561", // Buford Cocopoti
		"98499-2666", // Margaret Crumm
		"80112",      // Bens Iron Works
		"80904",      // Internal Medicine Clinic
		"21117"       // Mega Allied Services
		];

	internal static bool RunTest( IList<string?> counties, IList<string?> cities, IList<string?> postCodes )
	{
		if( AddressFactoryBase.Countries is null || AddressFactoryBase.Countries.Count == 0 )
		{
			_ = Program.sLogger.Info( "No Countries loaded." );
			return false;
		}
		else
		{
			_ = Program.sLogger.Info( $"Countries count..: {AddressFactoryBase.Countries.Count():###,###,###,###}" );

		}
		if( AddressFactoryBase.Provinces is null || AddressFactoryBase.Provinces.Count == 0 )
		{
			_ = Program.sLogger.Info( "No Provinces loaded." );
			return false;
		}
		else
		{
			_ = Program.sLogger.Info( $"Provinces count..: {AddressFactoryBase.Provinces.Count():###,###,###,###}" );

		}
		if( AddressFactoryBase.PostcodeCount == 0 )
		{
			_ = Program.sLogger.Info( "No Postcodes loaded." );
			return false;
		}
		else
		{
			_ = Program.sLogger.Info( $"Postcodes count..: {AddressFactoryBase.PostcodeCount:###,###,###,###}" );
		}

		var address = new Address() { Country = AddressFactoryBase.DefaultCountry };

		// Pick a random Province
		var rand = new Random();
		Province province = AddressFactoryBase.Provinces[rand.Next( AddressFactoryBase.Provinces.Count - 1 )];
		address.Province = province.Code;

		// Pick a random County
		if( counties.Count == 0 )
		{
			_ = Program.sLogger.Info( "No counties supplied." );
			return false;
		}
		address.County = counties[rand.Next( counties.Count - 1 )];

		// Pick a random City
		if( cities.Count == 0 )
		{
			_ = Program.sLogger.Info( "No cities supplied." );
			return false;
		}
		address.County = cities[rand.Next( cities.Count - 1 )];

		// Pick a random Postcode
		if( postCodes.Count == 0 )
		{
			_ = Program.sLogger.Info( "No postcodes supplied." );
			return false;
		}
		address.Postcode = postCodes[rand.Next( postCodes.Count - 1 )];

		return true;
	}

	private class Address : Common.Core.Models.Address
	{
		public string? County { get; set; }
	}
}