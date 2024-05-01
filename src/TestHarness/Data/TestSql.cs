using Common.Core.Models;
using Common.Data.Sql;

namespace TestHarness.Data;

internal class TestSql
{
	internal const string cTestDataDir = @"C:\Temp\TestData";

	internal static bool RunTest()
	{
		Console.WriteLine();
		_ = Program.sLogger.Info( "** Testing Common.Data.Sql (Microsoft.Data.SqlClient)" );
		_ = Program.sLogger.Info( "** AddressData, CompanyData, and PersonData database connection strings" );
		_ = Program.sLogger.Info( "** Uses SQL Server Express databases (AddressData.mdf and EntityData.mdf)" );
		Console.WriteLine();

		// Arrange address data
		_ = new AddressData( useAlpha2: false );
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
		//var data = companies.Get( cTestDataDir, "Company-test.json", max: 5 );
		//var data = companies.Get( cTestDataDir, "Company-test1.json", max: 15 );
		//_ = companies.Serialize( cTestDataDir, "Company-testout.json", data );

		People people = new( Program.sApp.ConfigFile );
		if( !Program.RunPeopleTests( people ) ) { return false; }
		//var data = people.Get( cTestDataDir, "Person-test.json", max: 5 );
		//var data = people.Get( cTestDataDir, "Person-test1.json", max: 15 );
		//_ = people.Serialize( cTestDataDir, "Person-testout.json", data );

		return true;
	}
}