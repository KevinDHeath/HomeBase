using Common.Core.Models;
using Common.Data.SqlServer;

namespace TestHarness.Data;

internal class TestSqlServer
{
	internal static bool RunTest()
	{
		Console.WriteLine();
		_ = Program.sLogger.Info( "** Testing Common.Data.SqlServer (EFCore.Database.Full)" );
		_ = Program.sLogger.Info( "** CommonData database connection string" );
		_ = Program.sLogger.Info( "** Uses SQL Server databases (TestEF)" );
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

		FullContextBase ctx = new();
		// Connection string in debug:
		// context => ChangeTracker => Context => Database => Non-Public members =>
		//  Dependencies => RelationalConnection

		Companies companies = new( ctx );
		if( !Program.RunCompaniesTests( companies ) ) { return false; }

		People people = new( ctx );
		if( !Program.RunPeopleTests( people ) ) { return false; }

		#region Test the Additional data

		Console.WriteLine();
		_ = Program.sLogger.Info( $"Movies total....: {ctx.Movies.Count():00#}" );

		// Get a specific Movie
		Common.Models.Movie? movie = ctx.Movies.Find( [5] );
		_ = Program.sLogger.Info( $"Movie title.....: {movie?.Title}" );

		_ = Program.sLogger.Info( $"SuperHero total.: {ctx.SuperHeroes.Count():00#}" );

		// Get a specific SuperHero
		Common.Models.SuperHero? hero = ctx.SuperHeroes.Find( [35] );
		_ = Program.sLogger.Info( $"SuperHero name..: {hero?.Name}" );

		#endregion

		return true;
	}
}