using Common.Core.Models;
using Common.Data.SQLite;

namespace TestHarness.Data;

internal class TestSQLite
{
	internal static bool RunTest()
	{
		Console.WriteLine();
		_ = Program.sLogger.Info( "** Testing Common.Data.SQLite (EFCore.Database.Entity)" );
		_ = Program.sLogger.Info( "** AddressDb and EntityDb database connection strings" );
		_ = Program.sLogger.Info( "** Uses SQLite databases (AddressData.db and EntityData.db)" );
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

		EntityContextBase ctx = new();
		// Connection string in debug:
		// context => ChangeTracker => Context => Database => Non-Public members =>
		//  Dependencies => RelationalConnection

		Companies companies = new( ctx );
		if( !Program.RunCompaniesTests( companies ) ) { return false; }

		People people = new( ctx );
		if( !Program.RunPeopleTests( people ) ) { return false; }

		return true;
	}
}
