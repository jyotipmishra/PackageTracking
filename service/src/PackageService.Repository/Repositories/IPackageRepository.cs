namespace PackageService.Repository.Repositories
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using PackageService.Domain.Models;
	using PackageService.Shared.Api;
	using PackageService.Shared.Enums;

	public interface IPackageRepository
	{	   
		Task CreatePackages(IEnumerable<Package> packages);

		Task<Package> GetPackageByTrackingCode(string trackingCode);

		Task<GetPagedResult<Package>> GetPackagesByStatusCode(ShipmentStatus statusCode, int page, int pageSize);

		Task<GetPagedResult<Package>> GetPackagesByPlaceType(PlaceType type, int page, int pageSize);

		Task UpdatePackage(Package package);
	}
}
