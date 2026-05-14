using Microsoft.EntityFrameworkCore;
using SpaceFlow.Api.Models;

namespace SpaceFlow.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<LeadClient> LeadsClients => Set<LeadClient>();
	public DbSet<Interaction> Interactions => Set<Interaction>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		base.OnModelCreating(modelBuilder);
	}
}
