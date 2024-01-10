using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Common.Core.Classes;
using Common.Core.Models;
using Common.Data.SQLite;

namespace EFCore.Database.Address;

/// <summary>Initializes a new instance of the AddressContext class.</summary>
public sealed class AddressContext() : AddressContextBase
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

	#endregion
}