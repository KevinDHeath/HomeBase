using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Common.Core.Classes;
using Common.Core.Models;

namespace Common.Data.SQLite;

/// <summary>Base class for an Entity database context.</summary>
/// <remarks>This uses connections strings contained in a Json application settings file.<br/>
/// The default file name is "appsettings.json".
/// <code language="JSON">
/// {
///   "ConnectionStrings": {
///     "EntityDb": "Data Source=[location]/[database].db"
///   }
/// }
/// </code>
/// </remarks>
public class EntityContextBase() : DbContext()
{
	/// <summary>Gets or sets the Companies data set.</summary>
	public DbSet<Company> Companies { get; set; }

	/// <summary>Gets or sets the People data set.</summary>
	public DbSet<Person> People { get; set; }

	internal static string? GetConnectionString( string name )
	{
		const string cConfigFilename = DataFactoryBase.cConfigFile;
		string? rtn = null;

		if( File.Exists( cConfigFilename ) )
		{
			IConfiguration config = new ConfigurationBuilder().AddJsonFile( cConfigFilename ).Build();
			rtn = config.GetValue<string>( $"ConnectionStrings:{name}" );
		}
		return rtn;
	}

	/// <summary>Configures the database to be used for this context.</summary>
	/// <param name="optionsBuilder">A builder used to create or modify options for this context.</param>
	protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
	{
		string? connStr = GetConnectionString( "EntityDb" );
		if( string.IsNullOrWhiteSpace( connStr ) ) { connStr = @"Data Source=.\Data\EntityData.db"; }
		_ = optionsBuilder.UseSqlite( connStr );
	}

	/// <summary>Configures the models for the entity types exposed in the datasets.</summary>
	/// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
	protected override void OnModelCreating( ModelBuilder modelBuilder )
	{
		ValueConverter<bool, char> converter = new( v => v ? 'Y' : 'N', v => v == 'Y' );
		_ = modelBuilder.Entity<Company>().Property( e => e.Private ).HasConversion( converter );
		_ = modelBuilder.Entity<Company>().OwnsOne( p => p.Address );

		_ = modelBuilder.Entity<Person>().OwnsOne( p => p.Address );
	}
}