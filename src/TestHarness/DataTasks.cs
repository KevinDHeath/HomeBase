using System;
using Common.Core.Classes;
//using Common.Data.Api;
//using Common.Data.Json;
using Common.Data.Sql;

namespace TestHarness;

internal static class DataTasks
{
	internal const string cTestDataDir = @"C:\Temp\TestData";

	#region Test Data

	internal static bool TestData()
	{
		Companies factory = new();
		var data = factory.Get( 5 );
		//var data = factory.Get( cTestDataDir, "Company-test.json", max: 5 );
		//var data = factory.Get( cTestDataDir, "Company-test1.json", max: 15 );
		//_ = factory.Serialize( cTestDataDir, "Company-testout.json", data );

		//People factory = new();
		//var data = factory.Get( 5 );
		//var data = factory.Get( cTestDataDir, "Person-test.json", max: 5 );
		//var data = factory.Get( cTestDataDir, "Person-test1.json", max: 15 );
		//_ = factory.Serialize( cTestDataDir, "Person-testout.json", data );

		return data.Count > 0;
	}

	#endregion

	#region Test Address Data

	internal static bool TestAddress()
	{
		var data = new AddressData( useAlpha2: false );
		if( AddressFactoryBase.Countries is null || AddressFactoryBase.Countries.Count == 0 ) { return false; }
		if( AddressFactoryBase.Provinces is null || AddressFactoryBase.Provinces.Count == 0 ) { return false; }
		if( AddressFactoryBase.PostcodeCount == 0 ) { return false; }

		var add = new Address()
		{
			//Postcode = "48908", // Katheryn A Tanguma
			//Postcode = "46204-3561", // Buford Cocopoti
			//Postcode = "98499-2666", // Margaret Crumm
			//Postcode = "80112", // Bens Iron Works
			//Postcode = "80904", // Internal Medicine Clinic
			//Postcode = "21117", // Mega Allied Services
			Country = AddressFactoryBase.DefaultCountry
		};

		// Get Postcode info based on user input
		var postcode = AddressData.GetPostcode( add.Postcode );
		if( postcode is not null )
		{
			add.Province = postcode.Province;
			add.County = postcode.County;
			add.City = postcode.City;
			return true;
		}

		// Pick a random Province name
		var rand = new Random();
		var province = AddressFactoryBase.Provinces[rand.Next( AddressFactoryBase.Provinces.Count - 1 )];

		// Get the code for selected Province name
		add.Province = province.Code;
		if( add.Province is null ) { return false; }

		// Pick a random County for the Province
		var counties = AddressData.GetCountyNames( add.Province );
		if( counties.Count == 0 ) { return false; }
		add.County = counties[rand.Next( counties.Count - 1 )];

		// Pick a random City for the Province and County
		var cities = AddressData.GetCityNames( add.Province, add.County );
		if( cities.Count == 0 ) { return false; }
		add.City = cities[rand.Next( cities.Count - 1 )];

		// Pick a random Postcode for the Province, County, and City
		var postcodes = AddressData.GetPostcodes( add.Province, city: add.City );
		if( postcodes.Count == 0 ) { return false; }
		add.Postcode = postcodes[rand.Next( postcodes.Count - 1 )];

		return data is not null;
	}

	private class Address : Common.Core.Models.Address
	{
		public string? County { get; set; }
	}

	#endregion
}