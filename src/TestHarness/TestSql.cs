using Common.Core.Classes;
using Common.Core.Models;
using Common.Data.Sql;

namespace TestHarness;

internal class TestSql : DataFactoryBase
{
	internal const string cTestDataDir = @"C:\Temp\TestData";

	internal static bool RunTest()
	{
		_ = Program.sLogger.Info( "Testing Common.Data.Sql (Microsoft.Data.SqkClient)" );
		_ = Program.sLogger.Info( "AddressData, CompanyData, and PersonData database connection strings" );
		_ = Program.sLogger.Info( "Uses SQL Server Express databases (AddressData.mdf and EntityData.mdf)" );
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
		Companies companies = new();
		People people = new();

		_ = Program.sLogger.Info( $"Companies count.: {companies.TotalCount:00#}" );
		_ = Program.sLogger.Info( $"People count....: {people.TotalCount:00#}" );
		Console.WriteLine();

		// Get a list of 5 Companies
		if( companies.TotalCount < 5 )
		{
			_ = Program.sLogger.Info( "Companies count is less than 5!" );
			return false;
		}
		var listc = companies.Get( 5 );
		_ = Program.sLogger.Info( $"Companies list..: {listc.Count:00#}" );

		//var data = companies.Get( cTestDataDir, "Company-test.json", max: 5 );
		//var data = companies.Get( cTestDataDir, "Company-test1.json", max: 15 );
		//_ = companies.Serialize( cTestDataDir, "Company-testout.json", data );

		// Get a list of 10 People
		if( people.TotalCount < 10 )
		{
			_ = Program.sLogger.Info( "People count is less than 10!" );
			return false;
		}
		var listp = people.Get( 10 );
		_ = Program.sLogger.Info( $"People list.....: {listp.Count:00#}" );

		//var data = people.Get( cTestDataDir, "Person-test.json", max: 5 );
		//var data = people.Get( cTestDataDir, "Person-test1.json", max: 15 );
		//_ = people.Serialize( cTestDataDir, "Person-testout.json", data );

		// Get a specific Company
		Company? company = companies.Find( listc[0].Id );
		_ = company is not null
			? Program.sLogger.Info( $"Company Id {listc[0].Id:00#} is {company.Name}" )
			: Program.sLogger.Info( $"Company Id {listc[0].Id:00#} not found!" );

		// Get a specific Person
		Person? person = people.Find( listp[0].Id );
		_ = person is not null
			? Program.sLogger.Info( $"Person Id  {listp[0].Id:00#} is {person.FullName}" )
			: Program.sLogger.Info( $"Person Id {listp[0].Id:00#} not found!" );

		return true;
	}
}