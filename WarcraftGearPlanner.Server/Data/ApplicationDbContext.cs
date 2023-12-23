using Microsoft.EntityFrameworkCore;
using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<ItemClassEntity> ItemClasses { get; set; } = null!;
	public DbSet<ItemSubclassEntity> ItemSubclasses { get; set; } = null!;
	public DbSet<RealmEntity> Realms { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		var entities = typeof(BaseEntity).Assembly.GetTypes()
			.Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(BaseEntity)));
		foreach (var entity in entities)
		{
			modelBuilder.Entity(entity).Property("CreatedAt").HasDefaultValueSql("GETUTCDATE()");
		}
	}
}
