using Application.Helper;
using Logging.Helper;

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
}