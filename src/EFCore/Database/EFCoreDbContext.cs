namespace EFCore.Database;

public class EFCoreDbContext : Data.EFCoreDbContext
{
	public EFCoreDbContext( DbContextOptions<Data.EFCoreDbContext> options )
		: base( options )
	{ }

	protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
		=> optionsBuilder.UseSqlServer( EFCoreDbContextFactory.GetSecret() );
}