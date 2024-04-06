using Common.Core.Models;
using Common.Data.Json;

namespace TestHarness.Data;

internal class TestJson
{
	internal static bool RunTest()
	{
		Console.WriteLine();
		_ = Program.sLogger.Info( "** Testing Common.Data.Json" );
		_ = Program.sLogger.Info( "** Uses embedded JSON resources" );
		Console.WriteLine();

		// Arrange address data
		_ = new AddressData();
		AddrParams args = new();
		foreach( string code in TestAddress.postcodes )
		{
			Postcode? pc = AddressData.GetPostcode( code );
			if( pc is not null )
			{
				args.Counties.AddRange( AddressData.GetCountyNames( pc.Province ) );
				args.Cities.AddRange( AddressData.GetCityNames( pc.Province, pc.County ) );
				args.Postcodes.AddRange( AddressData.GetPostcodes( pc.Province, pc.County, pc.City ) );
			}
		}

		if( !TestAddress.RunTest( args ) ) { return false; }
		Console.WriteLine();

		Companies companies = new( Program.sApp.ConfigFile );
		_ = companies.Get( 1 ); // Initializes collection on 1st Get
		if( !Program.RunCompaniesTests( companies ) ) { return false; }

		People people = new( Program.sApp.ConfigFile );
		_ = people.Get( 1 ); // Initializes collection on 1st Get
		if( !Program.RunPeopleTests( people ) ) { return false; }

		return true;
	}
}
