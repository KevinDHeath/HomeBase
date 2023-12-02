using System;
using Common.Core.Classes;
//using Common.Data.Api;
//using Common.Data.Json;
using Common.Data.Sql;

namespace TestHarness;

internal static class DataTasks
{
	#region Test Data

	internal const string cTestDataDir = @"C:\Users\Kevin\data\Test";

	internal static bool TestData()
	{
		//Companies factory = new();
		//var data = factory.Get( 5 );
		//var data = factory.Get( cTestDataDir, "Company-test.json", max: 5 );
		//_ = factory.Serialize( cTestDataDir, "Company-sql.json", data );

		People factory = new();
		var data = factory.Get( 5 );
		//var data = factory.Get( cTestDataDir, "Person-test.json", max: 5 );
		//_ = factory.Serialize( cTestDataDir, "Person-sql.json", data );

		return data.Count > 0;
	}

	#endregion

	#region Test Address Data

	internal static bool TestAddress()
	{
		var data = new AddressData( useAlpha2: false );
		if( AddressFactory.Countries is null || AddressFactory.Countries.Count == 0 ) { return false; }
		if( AddressFactory.States is null || AddressFactory.States.Count == 0 ) { return false; }
		if( AddressFactory.ZipCodeCount == 0 ) { return false; }

		var add = new Address()
		{
			//ZipCode = "48908", // Katheryn A Tanguma
			//ZipCode = "46204-3561", // Buford Cocopoti
			//ZipCode = "98499-2666", // Margaret Crumm
			//ZipCode = "80112", // Bens Iron Works
			//ZipCode = "80904", // Internal Medicine Clinic
			//ZipCode = "21117", // Mega Allied Services
			Country = AddressFactory.DefaultCountry
		};

		// Get Zip code info based on user input
		var zip = AddressData.GetZipCode( add.ZipCode );
		if( zip is not null )
		{
			add.State = zip.State;
			add.County = zip.County;
			add.City = zip.City;
			return true;
		}

		// Pick a random State name
		var rand = new Random();
		var state = AddressFactory.States[rand.Next( AddressFactory.States.Count - 1 )];

		// Get the code for selected State name
		add.State = state.Alpha;
		if( add.State is null ) { return false; }

		// Pick a random County for the State
		var counties = AddressData.GetCountyNames( add.State );
		if( counties.Count == 0 ) { return false; }
		add.County = counties[rand.Next( counties.Count - 1 )];

		// Pick a random City for the State and County
		var cities = AddressData.GetCityNames( add.State, add.County );
		if( cities.Count == 0 ) { return false; }
		add.City = cities[rand.Next( cities.Count - 1 )];

		// Pick a random Zip code for the State, County, and City
		var zipcodes = AddressData.GetZipCodes( add.State, city: add.City );
		if( zipcodes.Count == 0 ) { return false; }
		add.ZipCode = zipcodes[rand.Next( zipcodes.Count - 1 )];

		return data is not null;
	}

	private class Address : Common.Core.Models.Address
	{
		public string? County { get; set; }
	}

	#endregion
}