using Microsoft.EntityFrameworkCore;
using Common.Core.Models;

namespace Common.Data.SQLite;

/// <summary>Base class for an Address database context.</summary>
/// <remarks>This uses connections strings contained in a Json application settings file.<br/>
/// The default file name is "appsettings.json".
/// <code language="JSON">
/// {
///   "ConnectionStrings": {
///     "AddressDb": "Data Source=[location]/[database].db"
///   }
/// }
/// </code>
/// </remarks>
public class AddressContextBase() : DbContext()
{
	/// <summary>Gets or sets the Province data set.</summary>
	public DbSet<Province> Provinces { get; set; }

	/// <summary>Gets or sets the Postcode data set.</summary>
	public DbSet<Postcode> Postcodes { get; set; }

	/// <summary>Gets or sets the ISOCountry data set.</summary>
	public DbSet<ISOCountry> ISOCountries { get; set; }

	/// <summary>Configures the database to be used for this context.</summary>
	/// <param name="optionsBuilder">A builder used to create or modify options for this context.</param>
	protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
	{
		string? connStr = EntityContextBase.GetConnectionString( "AddressDb" );
		if( string.IsNullOrWhiteSpace( connStr ) ) { connStr = @"Data Source=.\Data\AddressData.db"; }
		_ = optionsBuilder.UseSqlite( connStr );

		base.OnConfiguring( optionsBuilder );
	}

	/// <summary>Configures the models for the entity types exposed in the datasets.</summary>
	/// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
	protected override void OnModelCreating( ModelBuilder modelBuilder )
	{
		_ = modelBuilder.Entity<ISOCountry>().HasIndex( b => b.Alpha2 );
		_ = modelBuilder.Entity<ISOCountry>().HasIndex( b => b.Alpha3 );
		_ = modelBuilder.Entity<ISOCountry>().Property( "Name" ).HasColumnType( "TEXT COLLATE NOCASE" );

		_ = modelBuilder.Entity<Postcode>().HasIndex( b => b.Code );
		_ = modelBuilder.Entity<Postcode>().Property( "City" ).HasColumnType( "TEXT COLLATE NOCASE" );
		_ = modelBuilder.Entity<Postcode>().Property( "County" ).HasColumnType( "TEXT COLLATE NOCASE" );

		_ = modelBuilder.Entity<Province>().HasIndex( b => b.Code );
		_ = modelBuilder.Entity<Province>().Property( "Name" ).HasColumnType( "TEXT COLLATE NOCASE" );
	}
}