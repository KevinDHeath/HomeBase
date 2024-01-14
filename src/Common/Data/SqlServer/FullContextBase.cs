using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Common.Core.Models;
using Common.Models;

namespace Common.Data.SqlServer;

/// <summary>Base class for a Full database context.</summary>
/// <remarks>This uses connections strings contained in a Json application settings file.<br/>
/// The default file name is "appsettings.json".
/// <code language="JSON">
/// {
///   "ConnectionStrings": {
///     "CommonData": "Server=XX;Database=YY;Trusted_Connection=true;TrustServerCertificate=true;"
///   }
/// }
/// </code>
/// </remarks>
public class FullContextBase : DbContext
{
	#region Constructors

	/// <summary>Initializes a new instance of the FullContextBase class.</summary>
	public FullContextBase() { }

	/// <summary>Initializes a new instance of the FullContextBase class with context options.</summary>
	/// <param name="options">Database context options.</param>
	public FullContextBase( DbContextOptions<FullContextBase> options ) : base( options ) { }

	#endregion

	#region Properties

	/// <summary>Gets or sets the Companies data set.</summary>
	public DbSet<Company> Companies { get; set; }

	/// <summary>Gets or sets the People data set.</summary>
	public DbSet<Person> People { get; set; }

	/// <summary>Gets or sets the ISOCountry data set.</summary>
	public DbSet<ISOCountry> ISOCountries { get; set; }

	/// <summary>Gets or sets the Postcode data set.</summary>
	public DbSet<Postcode> Postcodes { get; set; }

	/// <summary>Gets or sets the Province data set.</summary>
	public DbSet<Province> Provinces { get; set; }

	/// <summary>Gets or sets the SuperHero data set.</summary>
	public virtual DbSet<SuperHero> SuperHeroes { get; set; }

	/// <summary>Gets or sets the Movie data set.</summary>
	public virtual DbSet<Movie> Movies { get; set; }

	#endregion

	#region Private Methods

	private static string GetConnectionString( string name )
	{
		const string cConfigFilename = "appsettings.json";
		string? rtn = null;

		if( File.Exists( cConfigFilename ) )
		{
			IConfiguration config = new ConfigurationBuilder().AddJsonFile( cConfigFilename ).Build();
			rtn = config.GetValue<string>( $"ConnectionStrings:{name}" );
		}

		if( string.IsNullOrWhiteSpace( rtn ) )
		{
			rtn = @"Server=localhost;Database=EFCommonData;TrustServerCertificate=true;MultipleActiveResultSets=true;Integrated Security=SSPI;";
		}

		return rtn;
	}

	#endregion

	/// <inheritdoc />
	protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
	{
		string? connStr = GetConnectionString( "CommonData" );
		_ = optionsBuilder.UseSqlServer( connStr );
	}

	/// <inheritdoc />
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

		base.OnModelCreating( modelBuilder );
	}
}