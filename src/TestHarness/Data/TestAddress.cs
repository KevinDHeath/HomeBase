using Common.Core.Classes;
using Common.Core.Models;

namespace TestHarness.Data;

internal class TestAddress : DataFactoryBase
{
	#region Testing Postal codes

	internal static string[] postcodes = [
		"48908",      // Katheryn A Tanguma
		"46204-3561", // Buford Cocopoti
		"98499-2666", // Margaret Crumm
		"80112",      // Bens Iron Works
		"80904",      // Internal Medicine Clinic
		"21117"       // Mega Allied Services
		];

	#endregion

	internal static bool RunTest( AddrParams args )
	{
		#region Address Data Totals

		switch( AddressFactoryBase.Countries )
		{
			case not null when AddressFactoryBase.Countries.Count > 0:
				_ = Program.sLogger.Info( $"Countries total.: {AddressFactoryBase.Countries.Count:00#}" );
				break;
			default:
				_ = Program.sLogger.Info( "No Countries loaded!" );
				return false;
		}

		switch( AddressFactoryBase.Provinces )
		{
			case not null when AddressFactoryBase.Provinces.Count > 0:
				_ = Program.sLogger.Info( $"Provinces total.: {AddressFactoryBase.Provinces.Count:00#}" );
				break;
			default:
				_ = Program.sLogger.Info( "No Provinces loaded!" );
				return false;
		}

		switch( AddressFactoryBase.PostcodeCount )
		{
			case > 0:
				_ = Program.sLogger.Info( $"Postcodes total.: {AddressFactoryBase.PostcodeCount:#,00#}" );
				break;
			default:
				_ = Program.sLogger.Info( "No Postcodes loaded!" );
				return false;
		}

		switch( args.Counties.Count )
		{
			case > 0:
				_ = Program.sLogger.Info( $"County args.....: {args.Counties.Count:#,00#}" );
				break;
			default:
				_ = Program.sLogger.Info( "No Counties supplied!" );
				return false;
		}

		switch( args.Cities.Count )
		{
			case > 0:
				_ = Program.sLogger.Info( $"City args.......: {args.Cities.Count:#,00#}" );
				break;
			default:
				_ = Program.sLogger.Info( "No Cities supplied!" );
				return false;
		}

		switch( args.Postcodes.Count )
		{
			case > 0:
				_ = Program.sLogger.Info( $"Postcode args...: {args.Postcodes.Count:#,00#}" );
				break;
			default:
				_ = Program.sLogger.Info( "No Postcodes supplied!" );
				return false;
		}

		#endregion

		// Pick a random Postcode
		Postcode? postcode = AddressFactoryBase.Postcodes[sRandom.Next( 0, AddressFactoryBase.Postcodes.Count - 1 )];
		if( postcode is null )
		{
			_ = Program.sLogger.Info( "Random Postcode not found!" );
			return false;
		}

		string province = AddressFactoryBase.GetProvinceName( postcode.Province );
		if( string.IsNullOrWhiteSpace( province ) )
		{
			_ = Program.sLogger.Info( $"Province name for {postcode.Province} not found!" );
			return false;
		}

		Address address = new()
		{
			Country = AddressFactoryBase.DefaultCountry,
			City = postcode.City,
			Province = postcode.Province,
			Postcode = postcode.Code,

			// Pick a random County as the street
			Street = args.Counties[sRandom.Next( args.Counties.Count - 1 )]
		};

		_ = Program.sLogger.Info( $"Random address..: {address}, {province}, {address.Country} {address.Postcode}" );

		return true;
	}

	internal static bool RunTest()
	{
		// Use Json as that has all postal codes
		Common.Data.Json.AddressData data = new();

		// Min and Max Postal codes by Province
		foreach( var (s, min, max) in
				from s in Common.Data.Json.AddressData.GetPostcodes()
				let min = s.Min( z => z.Code )
				let max = s.Max( z => z.Code )
				select (s, min, max) )
		{
			_ = Program.sLogger.Info( $"Province: {s.Key} Min: {min} Max: {max}" );
		}

		// Pick a random Postal code
		Postcode? postCode = AddressFactoryBase.Postcodes[sRandom.Next( 0, AddressFactoryBase.Postcodes.Count - 1 )];

		// Postal codes by City name in all Provinces
		var query1 = from a in AddressFactoryBase.Postcodes
					 where a.City == postCode.City
					 orderby a.Province
					 group a by a.Province;
		int count1 = query1.Count();

		// Postal codes by City name and Province code
		var query2 = from province in AddressFactoryBase.Provinces
					 join postcode in AddressFactoryBase.Postcodes on province.Code equals postCode.Province
					 where postcode.City == postCode.City
					 select new
					 {
						State = province.Name,
						postcode.City,
						postcode.Code
					 };
		int count2 = query1.Count();

		return count1 > 0 && count2 > 0;
	}

	private class Address : Common.Core.Models.Address
	{
		public override string ToString() => Street + ", " + City;
	}
}

internal class AddrParams
{
	internal List<string?> Counties { get; set; } = new();

	internal List<string?> Cities { get; set; } = new ();

	internal List<string?> Postcodes { get; set; } = new();
}