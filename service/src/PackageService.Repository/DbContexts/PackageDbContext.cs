namespace PackageService.Repository.DbContexts
{
	using Microsoft.EntityFrameworkCore;
	using PackageService.Repository.Entities;

	public class PackageDbContext : DbContext
	{
		public PackageDbContext(DbContextOptions<PackageDbContext> option)
			: base(option)
		{
		}

		public DbSet<PackageEntity> Packages { get; set; }

		public DbSet<ShipmentEntity> Shipments { get; set; }

		public DbSet<CheckPointEntity> CheckPoints { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<PackageEntity>()
			   .HasIndex(p => new { p.TrackingCode })
			   .IsUnique();

			builder.Entity<PackageEntity>()
				.Property(p => p.ValueToPay)
				.HasColumnType("decimal(18,2)");
		}
	}
}
