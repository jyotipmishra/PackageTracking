namespace PackageService.Repository.Repositories
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using PackageService.Domain.Models;
	using PackageService.Repository.DbContexts;
	using PackageService.Repository.Mappers;
	using PackageService.Shared.Api;
	using PackageService.Shared.Enums;

	public class PackageRepository : IPackageRepository
	{
		private readonly PackageDbContext _dbcontext;

		public PackageRepository(PackageDbContext dbContext)
		{
			_dbcontext = dbContext;
		}

		public async Task CreatePackages(IEnumerable<Package> packages)
		{
			var packageEntities = packages
				.Select(p => p.ToEntity());

			await _dbcontext.AddRangeAsync(packageEntities);

			await _dbcontext.SaveChangesAsync();
		}

		public async Task<GetPagedResult<Package>> GetPackagesByStatusCode(
			ShipmentStatus statusCode,
			int page,
			int pageSize)
		{
			var packages = _dbcontext
				.Packages
				.AsNoTracking()
				.Include(p => p.Shipment)
				.ThenInclude(s => s.CheckPoint)
				.Where(p => p.Shipment.Status == statusCode)
				.Skip(page * pageSize)
				.Take(pageSize);

			var totalCount = await _dbcontext
				.Packages
				.CountAsync(p => p.Shipment.Status == statusCode);

			return new GetPagedResult<Package>(
				list: packages.Select(p => p.ToDomain()),
				totalCount: totalCount);
		}

		public async Task<Package> GetPackageByTrackingCode(string trackingCode)
		{
			var packageDetails = await _dbcontext
				.Packages
				.AsNoTracking()
				.Include(p => p.Shipment)
				.ThenInclude(s => s.CheckPoint)
				.SingleOrDefaultAsync(p => p.TrackingCode == trackingCode);

			return packageDetails.ToDomain();
		}

		public async Task<GetPagedResult<Package>> GetPackagesByPlaceType(PlaceType type, int page, int pageSize)
		{
			var packages = _dbcontext
				.Packages
				.AsNoTracking()
				.Include(p => p.Shipment)
				.ThenInclude(s => s.CheckPoint)
				.Where(p => p.Shipment.CheckPoint.PlaceType == type)
				.Skip(page * pageSize)
				.Take(pageSize);

			var totalCount = await _dbcontext
				.Packages
				.CountAsync(p => p.Shipment.CheckPoint.PlaceType == type);

			return new GetPagedResult<Package>(
				list: packages.Select(p => p.ToDomain()),
				totalCount: totalCount);
		}

		public async Task UpdatePackage(Package package)
		{
			_dbcontext.Packages.Update(package.ToEntity());

			await _dbcontext.SaveChangesAsync();
		}
	}
}
