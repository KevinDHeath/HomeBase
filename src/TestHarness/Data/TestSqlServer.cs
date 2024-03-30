using Common.Core.Interfaces;
using Common.Core.Models;
using Common.Data.SqlServer;

namespace TestHarness.Data;

internal class TestSqlServer
{
	internal static bool RunTest()
	{
		_ = Program.sLogger.Info( "Testing Common.Data.SqlServer (EFCore.Database.Full)" );
		_ = Program.sLogger.Info( "CommonData database connection string" );
		_ = Program.sLogger.Info( "Uses SQL Server databases (TestEF)" );
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
		Console.WriteLine();

		FullContextBase ctx = new();
		// Connection string in debug:
		// context => ChangeTracker => Context => Database => Non-Public members =>
		//  Dependencies => RelationalConnection

		#region Test the Company data

		Companies companies = new( ctx );
		_ = Program.sLogger.Info( $"Companies total.: {companies.TotalCount:00#}" );

		// Get a list of 5 Companies
		if( companies.TotalCount < 5 )
		{
			_ = Program.sLogger.Info( "Companies total is less than 5!" );
			return false;
		}
		IList<ICompany> listc = companies.Get( 5 );
		_ = Program.sLogger.Info( $"Companies list..: {listc.Count:00#}" );

		// Get a specific Company
		ICompany? company = companies.Find( listc[0].Id );
		_ = company is not null
			? Program.sLogger.Info( $"Company Id {listc[0].Id:00#} is {company.Name}" )
			: Program.sLogger.Info( $"Company Id {listc[0].Id:00#} not found!" );

		#endregion

		#region Test the Person data

		People people = new( ctx );
		_ = Program.sLogger.Info( $"People total....: {people.TotalCount:00#}" );

		// Get a list of 10 People
		if( people.TotalCount < 10 )
		{
			_ = Program.sLogger.Info( "People total is less than 10!" );
			return false;
		}
		IList<IPerson> listp = people.Get( 10 );
		_ = Program.sLogger.Info( $"People list.....: {listp.Count:00#}" );

		// Get a specific Person
		IPerson? person = people.Find( listp[0].Id );
		_ = person is not null
			? Program.sLogger.Info( $"Person Id  {listp[0].Id:00#} is {person.FullName}" )
			: Program.sLogger.Info( $"Person Id {listp[0].Id:00#} not found!" );

		#endregion

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