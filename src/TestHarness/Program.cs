using Application.Helper;
using Logging.Helper;
using Common.Core.Interfaces;

namespace TestHarness;

public class Program
{
	// Important: Set the application settings file Property in project to:
	// Copy to Output Directory = Copy if newer

	internal static readonly ConsoleApp sApp = new();
	internal static readonly Logger sLogger = new( typeof( Program ) );

	#region Program Entry Point

	static void Main( string[] args )
	{
		// To automatically close the console when debugging stops, in Visual Studio
		// enable Tools->Options->Debugging->Automatically close the console when debugging stops.

		var result = false;
		try
		{
			sApp.StartApp( args );
			sLogger.Info( sApp.FormatTitleLine( " Starting " + sApp.Title + " " ) );
			result = RunTest();
		}
		catch( Exception ex )
		{
			sLogger.Fatal( ex );
		}
		finally
		{
			sApp.StopApp( result );
		}

		Environment.Exit( Environment.ExitCode );
	}

	#endregion

	internal static bool RunTest()
	{
		if( !Data.TestAddress.RunTest() ) { return false; }
		if( !Data.TestAPI.RunTest() ) { return false; }
		if( !Data.TestJson.RunTest() ) { return false; }
		if( !Data.TestSql.RunTest() ) { return false; }
		if( !Data.TestSQLite.RunTest() ) { return false; }
		if( !Data.TestSqlServer.RunTest() ) { return false; }

		//if( !Json.TestConverters.RunTest() ) { return false; }

		return true;
	}

	internal static bool RunCompaniesTests( IDataFactory<ICompany> factory )
	{
		_ = sLogger.Info( $"Companies total.: {factory.TotalCount:00#}" );

		// Get a list of 5 Companies
		if( factory.TotalCount < 5 )
		{
			_ = sLogger.Info( "Companies count is less than 5!" );
			return false;
		}

		IList<ICompany> list = factory.Get( 5 );
		_ = sLogger.Info( $"Companies list..: {list.Count:00#}" );

		// Get a specific Company
		ICompany? company = factory.Find( list[0].Id );
		_ = company is not null
			? sLogger.Info( $"Company Id {list[0].Id:00#} is {company}" )
			: sLogger.Info( $"Company Id {list[0].Id:00#} not found!" );

		return true;
	}

	internal static bool RunPeopleTests( IDataFactory<IPerson> factory )
	{
		_ = sLogger.Info( $"People total....: {factory.TotalCount:00#}" );

		// Get a list of 10 People
		if( factory.TotalCount < 10 )
		{
			_ = Program.sLogger.Info( "People count is less than 10!" );
			return false;
		}
		IList<IPerson> list = factory.Get( 10 );
		_ = sLogger.Info( $"People list.....: {list.Count:00#}" );

		// Get a specific Person
		IPerson? person = factory.Find( list[0].Id );
		_ = person is not null
			? sLogger.Info( $"Person Id. {list[0].Id:00#} is {person.FullName}" )
			: sLogger.Info( $"Person Id. {list[0].Id:00#} not found!" );

		return true;
	}
}