using Common.Core.Interfaces;
using Common.Core.Models;
using Common.Data.SQLite;

namespace TestHarness;

internal class TestSQLite
{
	internal static bool RunTest()
	{
		_ = Program.sLogger.Info( "Testing Common.Data.SQLite (EFCore.Database.Entity)" );
		_ = Program.sLogger.Info( "AddressDb and EntityDb database connection strings" );
		_ = Program.sLogger.Info( "Uses SQLite databases (AddressData.db and EntityData.db)" );
		Console.WriteLine();

		// Test the address data
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

		// Test the entity data
		EntityContextBase ctx = new();
		// Connection string in debug:
		// context => ChangeTracker => Context => Database => Non-Public members =>
		//  Dependencies => RelationalConnection

		Companies companies = new( ctx );
		People people = new( ctx );

		_ = Program.sLogger.Info( $"Companies count.: {companies.TotalCount:00#}" );
		_ = Program.sLogger.Info( $"People count....: {people.TotalCount:00#}" );
		Console.WriteLine();

		// Get a list of 5 Companies
		if( companies.TotalCount < 5 )
		{
			_ = Program.sLogger.Info( "Companies count is less than 5" );
			return false;
		}
		IList<ICompany> listc = companies.Get( 5 );
		_ = Program.sLogger.Info( $"Companies list..: {listc.Count:00#}" );

		// Get a list of 10 People
		if( people.TotalCount < 10 )
		{
			_ = Program.sLogger.Info( "People count is less than 10" );
			return false;
		}
		IList<IPerson> listp = people.Get( 10 );
		_ = Program.sLogger.Info( $"People list.....: {listp.Count:00#}" );

		// Get a specific Company
		ICompany? company = companies.Find( listc[0].Id );
		_ = company is not null
			? Program.sLogger.Info( $"Company Id {listc[0].Id:00#} is {company.Name}" )
			: Program.sLogger.Info( $"Company Id {listc[0].Id:00#} not found." );

		// Get a specific Person
		IPerson? person = people.Find( listp[0].Id );
		_ = person is not null
			? Program.sLogger.Info( $"Person Id. {listp[0].Id:00#} is {person.FullName}" )
			: Program.sLogger.Info( $"Person Id. {listp[0].Id:00#} not found." );

		return true;
	}
}
