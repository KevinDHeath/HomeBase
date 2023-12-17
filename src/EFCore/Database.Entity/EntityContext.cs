using Microsoft.EntityFrameworkCore;
using Common.Data.SQLite;

namespace EFCore.Database.Entity;

/// <summary>Initializes a new instance of the EntityContext class.</summary>
public sealed class EntityContext() : EntityContextBase
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