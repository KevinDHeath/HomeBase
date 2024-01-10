using Common.Data.Api;

namespace TestHarness;

internal class TestAPI
{
	internal static bool RunTest()
	{
		_ = Program.sLogger.Info( "Testing Common.Data.Api" );
		_ = Program.sLogger.Info( "CommonData endpoint connection string" );
		_ = Program.sLogger.Info( " Uses EFCore.RestApi to EFCommonData database with Common user login." );
		Console.WriteLine();

		// Test the address data
		_ = new AddressData( Program.sApp.ConfigFile, useAlpha2: false );

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


		Companies companies = new( Program.sApp.ConfigFile );
		People people = new( Program.sApp.ConfigFile );

		_ = Program.sLogger.Info( $"Companies count..: {companies.TotalCount:###,###,###,###}" );
		_ = Program.sLogger.Info( $"People count.....: {people.TotalCount:###,###,###,###}" );


		//var postCode = AddressData.GetPostcode( "32937" );
		//_ = Program.sLogger.Info( $"Postcode City: {postCode?.City}" );

		// Get a specific company
		//var company = companies.
		//data = service.GetResource( company + "/43" );
		//if( data is null ) { return false; }
		//var obj0 = JsonHelper.DeserializeJson<ICompany>( ref data, Company.GetSerializerOptions() );
		//if( obj0 is not null )
		//{
		//	Program.sLogger.Info( $"Company name: {obj0.Name}" );
		//}

		var clist = companies.Get( 5 );
		// Get a list of companies
		//data = service.GetResource( company );
		//if( data is null ) { return false; }
		//var obj1 = JsonHelper.DeserializeJson<ResultsSet<ICompany>>( ref data, Company.GetSerializerOptions() );
		//if( obj1 is not null )
		//{
		//}

		// Get a list of 4 people
		var plist = people.Get( 4 );
		//data = service.GetResource( person + "?count=4&last=20" );
		//if( data is null ) { return false; }
		//var obj2 = JsonHelper.DeserializeJson<ResultsSet<IPerson>>( ref data, Person.GetSerializerOptions() );
		//if( obj2 is not null )
		//{
		//	Program.sLogger.Info( $"Next start: {obj2.Next}" );
		//}

		// Create a new SuperHero
		//var obj3 = new SuperHero { Name = "KevKogs", FirstName = "Test", Publisher = SuperHero.Publishers.Marvel };
		//obj3 = service.PostResource( superhero, obj3 );
		//if( obj3 is not null )
		//{
		//	Program.sLogger.Info( $"Superhero name: {obj3.Name}" );

		//	// Update the SuperHero
		//	obj3.Publisher = SuperHero.Publishers.DC;
		//	obj3 = service.PutResource( $"{superhero}/{obj3.Id}", obj3 );
		//	if( obj3 is not null )
		//	{
		//		Program.sLogger.Info( $"Superhero updated: {obj3.Publisher}" );

		//		// Delete the SuperHero
		//		obj3 = service.DeleteResource<SuperHero>( superhero + $"/{obj3?.Id}" );
		//		if( obj3 is not null )
		//		{
		//			Program.sLogger.Info( $"Superhero deleted: {obj3.Id}" );
		//		}
		//	}
		//}

		return true;
	}
}