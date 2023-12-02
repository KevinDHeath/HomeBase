global using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EFCore.Database;

public class EFCoreDbContextFactory : IDesignTimeDbContextFactory<EFCoreDbContext>
{
	private static string? _connectString;

	public EFCoreDbContext CreateDbContext( string[] args )
	{
		var optionsBuilder = new DbContextOptionsBuilder<Data.EFCoreDbContext>();
		optionsBuilder.UseSqlServer( GetSecret() );

		return new EFCoreDbContext( optionsBuilder.Options );
	}

	internal static string GetSecret()
	{
		if( _connectString is not null ) { return _connectString; }

		IConfiguration config = new ConfigurationBuilder()
			.AddUserSecrets<EFCoreDbContextFactory>()
			.Build();

		string? secret = config.GetValue<string>( "ConnectionStrings:CommonData" ) ??
			throw new Exception( "Could not retrieve connection string." );

		_connectString = secret;
		return _connectString;
	}
}