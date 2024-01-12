﻿using Common.Core.Classes;
using Common.Core.Models;
using Common.Data.SqlServer;
using Common.Data.SqlServer.Models;

namespace TestHarness;

internal class TestSqlServer : DataFactoryBase
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

		// Test the entity data
		FullContextBase ctx = new();

		// Connection string in debug:
		// context => ChangeTracker => Context =>
		//  Database => Non-Public members =>
		//  Dependencies => RelationalConnection

		_ = Program.sLogger.Info( $"Companies count.: {ctx.Companies.Count():00#}" );
		_ = Program.sLogger.Info( $"People count....: {ctx.People.Count():00#}" );
		_ = Program.sLogger.Info( $"Movies count....: {ctx.Movies.Count():00#}" );
		_ = Program.sLogger.Info( $"SuperHero count.: {ctx.SuperHeroes.Count():00#}" );
		Console.WriteLine();

		// Get a list of 5 Companies
		if( ctx.Companies.Count() < 5 )
		{
			_ = Program.sLogger.Info( "Companies count is less than 5!" );
			return false;
		}
		int startId = GetStartIndex( ctx.Companies.Count(), 5 );
		List<Company> listc = [.. ctx.Companies.Where( c => c.Id > startId ).Take( 5 )];
		_ = Program.sLogger.Info( $"Companies list..: {listc.Count:00#}" );

		// Get a list of 10 People
		if( ctx.People.Count() < 10 )
		{
			_ = Program.sLogger.Info( "People count is less than 10!" );
			return false;
		}
		startId = GetStartIndex( ctx.People.Count(), 10 );
		List<Company> listp = [.. ctx.Companies.Where( c => c.Id > startId ).Take( 10 )];
		_ = Program.sLogger.Info( $"People list.....: {listp.Count:00#}" );

		// Get a specific Company
		Company? company = ctx.Companies.Find( [listc[0].Id] );
		_ = company is not null
			? Program.sLogger.Info( $"Company Id {listc[0].Id:00#} is {company.Name}" )
			: Program.sLogger.Info( $"Company Id {listc[0].Id:00#} not found!" );

		// Get a specific Person
		Person? person = ctx.People.Find( [listp[0].Id] );
		_ = person is not null
			? Program.sLogger.Info( $"Person Id  {listp[0].Id:00#} is {person.FullName}" )
			: Program.sLogger.Info( $"Person Id  {listp[0].Id:00#} not found!" );

		SuperHero? hero = ctx.SuperHeroes.Find( [35] );
		_ = Program.sLogger.Info( $"SuperHero name..: {hero?.Name}" );

		return true;
	}
}