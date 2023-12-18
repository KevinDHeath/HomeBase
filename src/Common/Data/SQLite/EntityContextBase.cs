using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Common.Core.Models;

namespace Common.Data.SQLite;

/// <summary>Initializes a new instance of the EntityContextBase class.</summary>
public class EntityContextBase() : DbContext()
{
	/// <summary>Gets or sets the Companies data set.</summary>
	public DbSet<Company> Companies { get; set; }

	/// <summary>Gets or sets the People data set.</summary>
	public DbSet<Person> People { get; set; }

	internal static string? GetConnectionString( string name )
	{
		const string cConfigFilename = "appsettings.json";
		string? rtn = null;

		if( File.Exists( cConfigFilename ) )
		{
			IConfiguration config = new ConfigurationBuilder().AddJsonFile( cConfigFilename ).Build();
			rtn = config.GetValue<string>( $"ConnectionStrings:{name}" );
		}
		return rtn;
	}

	/// <inheritdoc />
	protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
	{
		string? connStr = GetConnectionString( "EntityDb" );
		if( string.IsNullOrWhiteSpace( connStr ) ) { connStr = @"Data Source=.\Data\EntityData.db"; }
		_ = optionsBuilder.UseSqlite( connStr );
	}

	/// <inheritdoc />
	protected override void OnModelCreating( ModelBuilder modelBuilder )
	{
		ValueConverter<bool, char> converter = new( v => v ? 'Y' : 'N', v => v == 'Y' );
		_ = modelBuilder.Entity<Company>().Property( e => e.Private ).HasConversion( converter );
		_ = modelBuilder.Entity<Company>().OwnsOne( p => p.Address );

		_ = modelBuilder.Entity<Person>().OwnsOne( p => p.Address );
	}
}