namespace PackageService.Tests.Infrastructure.Mapper
{
	using System.Linq;
	using AutoFixture;
	using FluentAssertions;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using PackageService.Domain.Models;
	using PackageService.Repository.Entities;
	using PackageService.Repository.Mappers;

	[TestClass]
	public class CheckPointMapperTests
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
			var checkPointEntity = _fixture.Create<CheckPointEntity>();

			var expected = new CheckPoint(
				checkPointId: checkPointEntity.CheckPointId,
				shipmentId: checkPointEntity.ShipmentId,
				city: checkPointEntity.City,
				country: checkPointEntity.Country,
				controlType: checkPointEntity.ControlType,
				placeType: checkPointEntity.PlaceType,
				createdAt: checkPointEntity.CreatedAt,
				updatedAt: checkPointEntity.UpdatedAt);

			//Act
			var actualCheckPointDomain = checkPointEntity.ToDomain();

			//Assert
			actualCheckPointDomain
				.Should()
				.BeEquivalentTo(expected);
		}

		[TestMethod]
		public void ToDomain_WithNullEntity_ShouldReturnNull()
		{
			//Arrange
			var checkPointEntity = null as ShipmentEntity;

			//Act
			var actualCheckPointDomain = checkPointEntity.ToDomain();

			//Assert
			actualCheckPointDomain
				.Should()
				.BeNull();
		}

		[TestMethod]
		public void ToEntity_WithValidDomain_ShouldMapCorrectly()
		{
			//Arrange
			var checkPoint = _fixture.Create<CheckPoint>();

			var expected = new CheckPointEntity
			{
				CheckPointId = checkPoint.CheckPointId,
				ShipmentId = checkPoint.ShipmentId.GetValueOrDefault(),
				City = checkPoint.City,
				Country = checkPoint.Country,
				ControlType = checkPoint.ControlType,
				PlaceType = checkPoint.PlaceType,
				CreatedAt = checkPoint.CreatedAt,
				UpdatedAt = checkPoint.UpdatedAt
			};

			//Act
			var actualCheckPointEntity = checkPoint.ToEntity();

			//Assert
			actualCheckPointEntity
				.Should()
				.BeEquivalentTo(expected);
		}

		[TestMethod]
		public void ToEntity_WithNullDomain_ShouldReturnNull()
		{
			//Arrange
			var checkPointDomain = null as CheckPoint;

			//Act
			var actualCheckPointEntity = checkPointDomain.ToEntity();

			//Assert
			actualCheckPointEntity
				.Should()
				.BeNull();
		}
	}
}
