using System;
using Logging.Helper;
using TestHarness.Json;

namespace TestHarness;

class Program
{
	// Important: Set the application settings file Property in project to:
	// Copy to Output Directory = Copy if newer

	internal static readonly OverrideConsoleApp sApp = new();
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
		//if( DataTasks.TestAddress() ) { return true; }
		//if( DataTasks.TestData() ) { return true; }
		//if( TestJson.RunTest() ) { return true; }
		if( TestLINQ.RunTest() ) { return true; }
		//if( Reflection.TestReflection.Test() ) { return true; }
		//if( TestRestAPI.RunTest() ) { return true; }

		return false;
	}
}