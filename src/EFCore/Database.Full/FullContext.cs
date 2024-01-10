using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Common.Core.Classes;
using Common.Core.Models;
using Common.Data.SqlServer;
using Common.Data.SqlServer.Models;

namespace Database.Full;

/// <summary>Initializes a new instance of the FullContext class.</summary>
public class FullContext : FullContextBase
{
	protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
	{
		base.OnConfiguring( optionsBuilder );
	}

	protected override void OnModelCreating( ModelBuilder modelBuilder )
	{
		base.OnModelCreating( modelBuilder );

		_ = modelBuilder.Entity<ISOCountry>().HasData( GetSeedDataISOCountry() );
		_ = modelBuilder.Entity<Postcode>().HasData( GetSeedDataPostcode() );
		_ = modelBuilder.Entity<Province>().HasData( GetSeedDataProvince() );
		_ = modelBuilder.Entity<Movie>().HasData( GetSeedDataMovie() );
		_ = modelBuilder.Entity<SuperHero>().HasData( GetSeedDataSuperHero() );
	}

	#region Data Seeding

	private const string cDataLocation = @"..\Data\Seed";

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
		if( !fi.Exists ) { Console.WriteLine( $"Could not find {fi.FullName}" ); return []; }
		string? json = File.ReadAllText( fi.FullName );
		return [.. JsonHelper.DeserializeList<ISOCountry>( ref json, SerializerOptions() )];
	}

	private static Postcode[] GetSeedDataPostcode()
	{
		FileInfo fi = new( Path.Combine( cDataLocation, "USPostCodes.json" ) );
		if( !fi.Exists ) { Console.WriteLine( $"Could not find {fi.FullName}" ); return []; }
		string? json = File.ReadAllText( fi.FullName );
		return [.. JsonHelper.DeserializeList<Postcode>( ref json, SerializerOptions() )];
	}

	private static Province[] GetSeedDataProvince()
	{
		FileInfo fi = new( Path.Combine( cDataLocation, "USProvinces.json" ) );
		if( !fi.Exists ) { Console.WriteLine( $"Could not find {fi.FullName}" ); return []; }
		string? json = File.ReadAllText( fi.FullName );
		return [.. JsonHelper.DeserializeList<Province>( ref json, SerializerOptions() )];
	}

	private static Movie[] GetSeedDataMovie()
	{
		FileInfo fi = new( Path.Combine( cDataLocation, "Movies.json" ) );
		if( !fi.Exists ) { Console.WriteLine( $"Could not find {fi.FullName}" ); return []; }
		string? json = File.ReadAllText( fi.FullName );
		return [.. JsonHelper.DeserializeList<Movie>( ref json, SerializerOptions() )];
	}

	private static SuperHero[] GetSeedDataSuperHero()
	{
		FileInfo fi = new( Path.Combine( cDataLocation, "SuperHeroes.json" ) );
		if( !fi.Exists ) { Console.WriteLine( $"Could not find {fi.FullName}" ); return []; }
		string? json = File.ReadAllText( fi.FullName );
		return [.. JsonHelper.DeserializeList<SuperHero>( ref json, SerializerOptions() )];
	}

	#endregion
}