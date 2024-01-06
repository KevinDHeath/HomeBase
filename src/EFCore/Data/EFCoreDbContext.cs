using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Common.Core.Classes;
using Common.Core.Models;
using EFCore.Data.Models;

namespace EFCore.Data;

/// <summary>Initializes a new instance of the EFCoreDbContext class.</summary>
/// <param name="options">Database context options.</param>
[method: EditorBrowsable( EditorBrowsableState.Never )]
public class EFCoreDbContext( DbContextOptions<EFCoreDbContext> options ) : DbContext( options )
{
	#region Properties

	/// <summary>Gets or sets the Companies data set.</summary>
	public DbSet<Company> Companies { get; set; }

	/// <summary>Gets or sets the People data set.</summary>
	public virtual DbSet<Person> People { get; set; }

	/// <summary>Gets or sets the Province data set.</summary>
	public virtual DbSet<Province> Provinces { get; set; }

	/// <summary>Gets or sets the Postcode data set.</summary>
	public virtual DbSet<Postcode> Postcodes { get; set; }

	/// <summary>Gets or sets the ISOCountry data set.</summary>
	public virtual DbSet<ISOCountry> ISOCountries { get; set; }

	/// <summary>Gets or sets the SuperHero data set.</summary>
	public virtual DbSet<SuperHero> SuperHeroes { get; set; }

	/// <summary>Gets or sets the Movie data set.</summary>
	public virtual DbSet<Movie> Movies { get; set; }

	#endregion

	#region Overridden Methods

	/// <summary>
	/// Override this method to set defaults and configure conventions before they run.
	/// This method is invoked before OnModelCreating(ModelBuilder).
	/// </summary>
	/// <param name="builder">The builder being used to set defaults and configure conventions
	/// that will be used to build the model for this context.</param>
	protected override void ConfigureConventions( ModelConfigurationBuilder builder )
	{
		base.ConfigureConventions( builder );
	}

	/// <summary>
	/// Override this method to further configure the model that was discovered by convention
	/// from the entity types exposed in DbSet&lt;TEntity&gt; properties on the derived context.
	/// The resulting model may be cached and re-used for subsequent instances of the derived context.
	/// </summary>
	/// <param name="modelBuilder">The builder being used to construct the model for this context.
	/// Databases (and other extensions) typically define extension methods on this object that
	/// allow you to configure aspects of the model that are specific to a given database.</param>
	protected override void OnModelCreating( ModelBuilder modelBuilder )
	{
		ValueConverter<bool, char> converter = new( v => v ? 'Y' : 'N', v => v == 'Y' );
		_ = modelBuilder.Entity<Company>().Property( e => e.Private ).HasConversion( converter );
		_ = modelBuilder.Entity<Company>().OwnsOne( p => p.Address );

		_ = modelBuilder.Entity<Person>().OwnsOne( p => p.Address );

		_ = modelBuilder.Entity<ISOCountry>().HasIndex( b => b.Alpha2 );
		_ = modelBuilder.Entity<ISOCountry>().HasIndex( b => b.Alpha3 );
		_ = modelBuilder.Entity<Province>().HasIndex( b => b.Code );
		_ = modelBuilder.Entity<Postcode>().HasIndex( b => b.Code );

		_ = modelBuilder.Entity<SuperHero>().Property( e => e.Publisher ).HasColumnType( @"nvarchar(20)" );
		_ = modelBuilder.Entity<SuperHero>().HasIndex( b => b.Name );

		_ = modelBuilder.Entity<Movie>().HasIndex( b => b.Title );

		_ = modelBuilder.Entity<ISOCountry>().HasData( GetSeedDataISOCountry() );
		_ = modelBuilder.Entity<Postcode>().HasData( GetSeedDataPostcode() );
		_ = modelBuilder.Entity<Province>().HasData( GetSeedDataProvince() );

		base.OnModelCreating( modelBuilder );
	}

	#endregion

	#region Data Seeding

	private const string cDataLocation = @"..\Database.Address\Data";

	private static JsonSerializerOptions SerializerOptions()
	{
		JsonSerializerOptions rtn = JsonHelper.DefaultSerializerOptions();
		rtn.NumberHandling = JsonNumberHandling.AllowReadingFromString;
		rtn.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create( System.Text.Unicode.UnicodeRanges.BasicLatin );
		return rtn;
	}

	private static ISOCountry[] GetSeedDataISOCountry()
	{
		FileInfo fi = new( Path.Combine( cDataLocation, "ISOCountries.json" ) );
		if( !fi.Exists ) { return []; }
		string? json = File.ReadAllText( fi.FullName );
		return [.. JsonHelper.DeserializeList<ISOCountry>( ref json, SerializerOptions() )];
	}

	private static Postcode[] GetSeedDataPostcode()
	{
		FileInfo fi = new( Path.Combine( cDataLocation, "USPostCodes.json" ) );
		if( !fi.Exists ) { return []; }
		string? json = File.ReadAllText( fi.FullName );
		return [.. JsonHelper.DeserializeList<Postcode>( ref json, SerializerOptions() )];
	}

	private static Province[] GetSeedDataProvince()
	{
		FileInfo fi = new( Path.Combine( cDataLocation, "USProvinces.json" ) );
		if( !fi.Exists ) { return []; }
		string? json = File.ReadAllText( fi.FullName );
		return [.. JsonHelper.DeserializeList<Province>( ref json, SerializerOptions() )];
	}

	#endregion
}