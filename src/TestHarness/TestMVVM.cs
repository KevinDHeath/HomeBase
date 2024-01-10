using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Common.Core.Models;
using Common.Data.SQLite;
using MVVM.Core.Services;
using MVVM.Core.Stores;

namespace TestHarness;

internal class TestMVVM
{
	internal static bool RunTest()
	{
		//TestContext();
		TestServices();

		return true;
	}

	internal static void TestServices()
	{
		IServiceProvider _serviceProvider = ServiceProviderHelper.Create();
		PeopleStore store = _serviceProvider.GetRequiredService<PeopleStore>();
		_ = Program.sLogger.Info( $"Records in store: {store.Count}" );

		Postcode? postCode = AddressData.GetPostcode( "85013" );
		_ = Program.sLogger.Info( $"Postcode county: {postCode?.County}" );
	}

	internal static void TestContext()
	{
		using EntityContextBase context = new();

		_ = context.Database.EnsureCreated();

		Company? company = context.Companies.FirstOrDefault( c => c.Id == 1 );
		if( company is null )
		{
			context.Companies.Add( new Company { Id = 1, Name = "Test Add" } );
			context.SaveChanges();
		}

		foreach( Company rec in context.Companies.Include( c => c.Address ).OrderBy( c => c.Name ) )
		{
			_ = Program.sLogger.Info( $"{rec.Name}: {rec.Address.FullAddress}" );
		}
	}
}
