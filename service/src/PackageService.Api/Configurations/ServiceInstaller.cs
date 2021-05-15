namespace PackageService.Api.Configuration
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using PackageService.Repository.DbContexts;
	using PackageService.Repository.Repositories;

	public static class ServicesInstaller
	{
		public static IServiceCollection RegisterDependencies(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			return services
				.AddDbContext<PackageDbContext>(
				options => options.UseSqlServer(
					configuration.GetConnectionString("PackageServiceConnectionString")))
				.AddScoped<IPackageRepository, PackageRepository>();
		}
	}
}
