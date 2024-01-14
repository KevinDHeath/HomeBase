using Common.Core.Classes;
using Common.Core.Models;

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