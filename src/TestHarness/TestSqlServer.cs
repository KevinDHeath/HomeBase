using Common.Data.SqlServer;
using Common.Data.SqlServer.Models;

namespace TestHarness;

internal class TestSqlServer
{
	internal static bool RunTest()
	{
		_ = Program.sLogger.Info( "Testing Common.Data.SqlServer" );
		_ = Program.sLogger.Info( "CommonData database connection string" );
		_ = Program.sLogger.Info( "Uses SQL Server databases (TestEF)" );
		Console.WriteLine();

		FullContextBase full = new();

		// Connection string in debug:
		// context => ChangeTracker => Context =>
		//  Database => Non-Public members =>
		//  Dependencies => RelationalConnection

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

		_ = Program.sLogger.Info( $"Companies count..: {full.Companies.Count():###,###,###,###}" );
		_ = Program.sLogger.Info( $"People count.....: {full.People.Count():###,###,###,###}" );
		_ = Program.sLogger.Info( $"Movies count.....: {full.Movies.Count():###,###,###,###}" );
		_ = Program.sLogger.Info( $"SuperHeroes count: {full.SuperHeroes.Count():###,###,###,###}" );

		Console.WriteLine();

		SuperHero? hero = full.SuperHeroes.Find( [35] );
		_ = Program.sLogger.Info( $"SuperHero name...: {hero?.Name}" );

		return true;
	}
}