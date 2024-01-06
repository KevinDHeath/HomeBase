using Common.Core.Classes;
using Common.Core.Interfaces;
using Common.Core.Models;
using EFCore.Data.Models;

namespace TestHarness;

internal class TestRestAPI
{
	internal static bool RunTest()
	{
		//if( AddressDataTest() ) { return true; }
		if( SimpleTests( CreateDataService() ) ) { return true; }

		return false;
	}

	internal static DataServiceBase CreateDataService()
	{
		DataServiceBase rtn = new( "http://localhost:8084/api/" );
		return rtn;
	}

	#region Simple Tests

	internal static bool SimpleTests( DataServiceBase service )
	{
		string company = typeof( Company ).Name.ToLower();
		string person = typeof( Person ).Name.ToLower();
		string postcode = typeof( Postcode ).Name.ToLower();
		string superhero = typeof( SuperHero ).Name.ToLower();

		string? data = service.GetResource( postcode + "/32937" );
		if( data is null ) { return false; }
		var obj = JsonHelper.DeserializeJson<Postcode>( ref data );
		if( obj is not null )
		{
			Program.sLogger.Info( $"Postcode City: {obj.City}" );
		}

		// Get a specific company
		data = service.GetResource( company + "/43" );
		if( data is null ) { return false; }
		var obj0 = JsonHelper.DeserializeJson<ICompany>( ref data, Company.GetSerializerOptions() );
		if( obj0 is not null )
		{
			Program.sLogger.Info( $"Company name: {obj0.Name}" );
		}

		// Get a list of companies
		data = service.GetResource( company );
		if( data is null ) { return false; }
		var obj1 = JsonHelper.DeserializeJson<ResultsSet<ICompany>>( ref data, Company.GetSerializerOptions() );
		if( obj1 is not null )
		{
			Program.sLogger.Info( $"Company count: {obj1.Total}" );
		}

		// Get a list of 4 people
		data = service.GetResource( person + "?count=4&last=20" );
		if( data is null ) { return false; }
		var obj2 = JsonHelper.DeserializeJson<ResultsSet<IPerson>>( ref data, Person.GetSerializerOptions() );
		if( obj2 is not null )
		{
			Program.sLogger.Info( $"Next start: {obj2.Next}" );
		}

		// Create a new SuperHero
		var obj3 = new SuperHero { Name = "KevKogs", FirstName = "Test", Publisher = SuperHero.Publishers.Marvel };
		obj3 = service.PostResource( superhero, obj3 );
		if( obj3 is not null )
		{
			Program.sLogger.Info( $"Superhero name: {obj3.Name}" );

			// Update the SuperHero
			obj3.Publisher = SuperHero.Publishers.DC;
			obj3 = service.PutResource( $"{superhero}/{obj3.Id}", obj3 );
			if( obj3 is not null )
			{
				Program.sLogger.Info( $"Superhero updated: {obj3.Publisher}" );

				// Delete the SuperHero
				obj3 = service.DeleteResource<SuperHero>( superhero + $"/{obj3?.Id}" );
				if( obj3 is not null )
				{
					Program.sLogger.Info( $"Superhero deleted: {obj3.Id}" );
				}
			}
		}

		return true;
	}

	#endregion

	#region Test AddressData

	internal static bool AddressDataTest()
	{
		var data = new Common.Data.Api.AddressData( useAlpha2: false );
		_ = Common.Data.Api.AddressData.GetPostcode( "32937" );

		return data is not null;
	}

	#endregion
}