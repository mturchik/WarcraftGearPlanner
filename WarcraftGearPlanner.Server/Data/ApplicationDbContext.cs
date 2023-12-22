using Microsoft.EntityFrameworkCore;
using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<ItemClassEntity> ItemClasses { get; set; } = null!;
	public DbSet<ItemSubclassEntity> ItemSubclasses { get; set; } = null!;
}
// WarcraftGearPlanner.Server.Data.ApplicationDbContext