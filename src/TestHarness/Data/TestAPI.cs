using Common.Core.Models;
using Common.Data.Api;

namespace TestHarness.Data;

internal class TestAPI
{
	internal static bool RunTest()
	{
		Console.WriteLine();
		_ = Program.sLogger.Info( "** Testing Common.Data.Api (EFCore.RestApi)" );
		_ = Program.sLogger.Info( "** CommonData endpoint connection string" );
		_ = Program.sLogger.Info( "** Uses EFCommonData database with Common user login." );
		Console.WriteLine();

		// Arrange address data
		_ = new AddressData( Program.sApp.ConfigFile, useAlpha2: false );
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
		if( !Program.RunCompaniesTests( companies ) ) { return false; }

		People people = new( Program.sApp.ConfigFile );
		if( !Program.RunPeopleTests( people ) ) { return false; }

		return true;
	}
}