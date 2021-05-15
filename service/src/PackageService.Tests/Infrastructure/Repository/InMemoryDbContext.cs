namespace PackageService.Infrastructure.Tests.Repositories
{
	using System;
	using Microsoft.EntityFrameworkCore;
	using PackageService.Repository.DbContexts;

	public class InMemoryDbContext
{
        public static PackageDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<PackageDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
            var dbContext = new PackageDbContext(options);
            return dbContext;
        }
    }
}