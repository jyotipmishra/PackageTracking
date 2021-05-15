namespace PackageService.Tests.Infrastructure.Repository
{
	using System.Linq;
	using System.Threading.Tasks;
	using AutoFixture;
	using FluentAssertions;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using PackageService.Domain.Models;
	using PackageService.Infrastructure.Tests.Repositories;
	using PackageService.Repository.Entities;
	using PackageService.Repository.Mappers;
	using PackageService.Repository.Repositories;
	using PackageService.Shared.Enums;

	[TestClass]
	public class PackageRepositoryTests
	{
		private Fixture _fixture;

		[TestInitialize]
		public void TestInitialize()
		{
			_fixture = new Fixture();
			_fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
				.ForEach(b => _fixture.Behaviors.Remove(b));
			_fixture.Behaviors.Add(new OmitOnRecursionBehavior());
		}

		[TestMethod]
		public async Task CreatePackages_WithValidData_PackagesCreatedSuccessfully()
		{
			//Arrange
			var packages = _fixture.CreateMany<Package>(1000);

			var dbContext = InMemoryDbContext.CreateDbContext();

			var packageRepository = new PackageRepository(dbContext);

			//Act
			await packageRepository.CreatePackages(packages);

			//Assert
			var expected = dbContext.Packages.Count();

			packages.Count()
				.Should()
				.Be(expected);
		}

		[TestMethod]
		public async Task GetPackagesByStatusCode_WithValidStatus_ReturnsPackagesSuccessfully()
		{
			//Arrange
			var packages = _fixture.CreateMany<PackageEntity>(2000);

			var dbContext = InMemoryDbContext.CreateDbContext();

			var packageRepository = new PackageRepository(dbContext);
			await dbContext.AddRangeAsync(packages);
			await dbContext.SaveChangesAsync();

			//Act
			var result = await packageRepository
				.GetPackagesByStatusCode(
				statusCode: 0,
				page: 0,
				pageSize: 50);

			//Assert
			result.List.Count()
				.Should()
				.BeGreaterThan(0);

			result.TotalCount
				.Should()
				.BeGreaterThan(0);
		}

		[TestMethod]
		public async Task GetPackageByTrackingCode_WithValidCode_ReturnsPackageSuccessfully()
		{
			//Arrange
			var trackingCode = _fixture.Create<string>();

			var package = _fixture
				.Build<PackageEntity>()
				.With(p => p.TrackingCode, trackingCode)
				.Create();

			var dbContext = InMemoryDbContext.CreateDbContext();

			var packageRepository = new PackageRepository(dbContext);
			await dbContext.AddAsync(package);
			await dbContext.SaveChangesAsync();

			//Act
			var result = await packageRepository
				.GetPackageByTrackingCode(trackingCode);

			//Assert
			result
				.Should()
				.NotBeNull();

			result.TrackingCode
				.Should()
				.Be(trackingCode);
		}

		[TestMethod]
		public async Task GetPackagesByPlaceType_WithValidStatus_ReturnsPackagesSuccessfully()
		{
			//Arrange
			var packages = _fixture.CreateMany<PackageEntity>(2000);

			var dbContext = InMemoryDbContext.CreateDbContext();

			var packageRepository = new PackageRepository(dbContext);
			await dbContext.AddRangeAsync(packages);
			await dbContext.SaveChangesAsync();

			//Act
			var result = await packageRepository
				.GetPackagesByPlaceType(
				type: 0,
				page: 0,
				pageSize: 50);

			//Assert
			result.List.Count()
				.Should()
				.BeGreaterThan(0);

			result.TotalCount
				.Should()
				.BeGreaterThan(0);
		}

		[TestMethod]
		public async Task UpdatePackage_WithUpdatedData_PackageUpdatedSuccessfully()
		{
			//Arrange
			var trackingCode = _fixture.Create<string>();

			var packageEntity = _fixture
				.Build<PackageEntity>()
				.With(p => p.TrackingCode, trackingCode)
				.Create();

			var dbContext = InMemoryDbContext.CreateDbContext();

			var packageRepository = new PackageRepository(dbContext);
			await dbContext.AddAsync(packageEntity);
			await dbContext.SaveChangesAsync();
			dbContext.Entry(packageEntity).State = EntityState.Detached;

			var packageToUpdate = packageEntity.ToDomain();

			packageToUpdate.Shipment.Update(
				false,
				ShipmentStatus.Delivered);

			//Act
			await packageRepository.UpdatePackage(packageToUpdate);

			//Assert
			var updatedPackage = await packageRepository
				.GetPackageByTrackingCode(trackingCode);

			updatedPackage.Shipment.Status
				.Should()
				.Be(ShipmentStatus.Delivered);

			updatedPackage.Shipment.IsStoppedInCustoms
				.Should()
				.Be(false);
		}
	}
}
