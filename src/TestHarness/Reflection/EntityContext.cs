using Microsoft.EntityFrameworkCore;

namespace Common.Data.Entity;

/// <summary>Initializes a new instance of the EntityContext class.</summary>
public sealed class EntityContext() : Common.Data.SQLite.EntityContext
{
	protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
	{
		base.OnConfiguring( optionsBuilder );
	}

	protected override void OnModelCreating( ModelBuilder modelBuilder )
	{
		base.OnModelCreating( modelBuilder );
	}
}