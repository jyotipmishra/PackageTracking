namespace PackageService.Tests.Infrastructure.Mapper
{
	using System;
	using System.Linq;
	using AutoFixture;
	using FluentAssertions;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using PackageService.Domain.Models;
	using PackageService.Repository.Entities;
	using PackageService.Repository.Mappers;

	[TestClass]
	public class PackageMapperTests
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
		public void ToDomain_WithValidEntity_ShouldMapCorrectly()
		{
			//Arrange
			var packageEntity = _fixture.Create<PackageEntity>();

			var expected = new Package(
				id: packageEntity.PackageId,
				size: packageEntity.Size,
				weight: packageEntity.Weight,
				isFragile: packageEntity.IsFragile,
				valueToPay: packageEntity.ValueToPay,
				trackingCode: packageEntity.TrackingCode,
				shipment: packageEntity.Shipment.ToDomain(),
				version: packageEntity.Version,
				createdAt: packageEntity.CreatedAt,
				updatedAt: packageEntity.UpdatedAt);

			//Act
			var actualPackageDomain = packageEntity.ToDomain();

			//Assert
			actualPackageDomain
				.Should()
				.BeEquivalentTo(expected);
		}

		[TestMethod]
		public void ToDomain_WithNullEntity_ShouldReturnNull()
		{
			//Arrange
			var packageEntity = null as PackageEntity;

			//Act
			var actualPackageDomain = packageEntity.ToDomain();

			//Assert
			actualPackageDomain
				.Should()
				.BeNull();
		}

		[TestMethod]
		public void ToEntity_WithValidDomain_ShouldMapCorrectly()
		{
			//Arrange
			var package = _fixture.Create<Package>();

			var expected = new PackageEntity
			{
				PackageId = package.PackageId,
				Size = package.Size,
				Weight = package.Weight,
				IsFragile = package.IsFragile,
				ValueToPay = package.ValueToPay,
				TrackingCode = package.TrackingCode,
				Shipment =  package.Shipment.ToEntity(),
				Version = Guid.NewGuid(),
				CreatedAt = package.CreatedAt,
				UpdatedAt = package.UpdatedAt
			};

			//Act
			var actualPackageEntity = package.ToEntity();

			//Assert
			actualPackageEntity
				.Should()
				.BeEquivalentTo(expected, option => 
				option.Excluding(a => a.Version));
		}

		[TestMethod]
		public void ToEntity_WithNullDomain_ShouldReturnNull()
		{
			//Arrange
			var packageDomain = null as Package;

			//Act
			var actualPackageEntity = packageDomain.ToEntity();

			//Assert
			actualPackageEntity
				.Should()
				.BeNull();
		}
	}
}
