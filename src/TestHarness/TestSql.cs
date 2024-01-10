//using Common.Data.Json;
using Common.Data.Sql;

namespace TestHarness;

internal static class TestSql
{
	internal const string cTestDataDir = @"C:\Temp\TestData";

	internal static bool RunTest()
	{
		_ = Program.sLogger.Info( "Testing Common.Data.Sql" );
		_ = Program.sLogger.Info( "AddressData, CompanyData, and PersonData database connection strings" );
		_ = Program.sLogger.Info( "Uses SQL Server Express databases (AddressData and EntityData)" );
		Console.WriteLine();

		// Test the address data
		_ = new AddressData( useAlpha2: false );

		List<string?> counties = new();
		List<string?> cities = new();
		List<string?> postCodes = new();
		foreach( string code in TestAddress.postcodes )
		{
			var post = AddressData.GetPostcode( code );
			counties.AddRange( AddressData.GetCountyNames( post?.Province ) );
			cities.AddRange( AddressData.GetCityNames( post?.Province, post?.County ) );
			postCodes.AddRange( AddressData.GetPostcodes( post?.Province, post?.County, post?.City ) );
		}

		if( !TestAddress.RunTest( counties, cities, postCodes ) ) { return false; }

		Companies comps = new();
		People peeps = new();

		_ = Program.sLogger.Info( $"Companies count..: {comps.TotalCount:###,###,###,###}" );
		_ = Program.sLogger.Info( $"People count.....: {peeps.TotalCount:###,###,###,###}" );

		var datac = comps.Get( 5 );
		//var data = compss.Get( cTestDataDir, "Company-test.json", max: 5 );
		//var data = compss.Get( cTestDataDir, "Company-test1.json", max: 15 );
		//_ = comps.Serialize( cTestDataDir, "Company-testout.json", data );

		var datap = peeps.Get( 5 );
		//var data = peeps.Get( cTestDataDir, "Person-test.json", max: 5 );
		//var data = peeps.Get( cTestDataDir, "Person-test1.json", max: 15 );
		//_ = peeps.Serialize( cTestDataDir, "Person-testout.json", data );

		return true;
	}
}