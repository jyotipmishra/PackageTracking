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
	public class ShipmentMapperTests
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
			var shipmentEntity = _fixture.Create<ShipmentEntity>();

			var expected = new Shipment(
				shipmentId: shipmentEntity.ShipmentId,
				packageId: shipmentEntity.PackageId,
				isStoppedInCustom: shipmentEntity.IsStoppedInCustoms,
				status: shipmentEntity.Status,
				receivedDate: shipmentEntity.ReceivedDate,
				checkPoint: shipmentEntity.CheckPoint.ToDomain(),
				createdAt: shipmentEntity.CreatedAt,
				updatedAt: shipmentEntity.UpdatedAt);

			//Act
			var actualShipmentDomain = shipmentEntity.ToDomain();

			//Assert
			actualShipmentDomain
				.Should()
				.BeEquivalentTo(expected);
		}

		[TestMethod]
		public void ToDomain_WithNullEntity_ShouldReturnNull()
		{
			//Arrange
			var shipmentEntity = null as ShipmentEntity;

			//Act
			var actualShipmentDomain = shipmentEntity.ToDomain();

			//Assert
			actualShipmentDomain
				.Should()
				.BeNull();
		}

		[TestMethod]
		public void ToEntity_WithValidDomain_ShouldMapCorrectly()
		{
			//Arrange
			var shipment = _fixture.Create<Shipment>();

			var expected = new ShipmentEntity
			{
				ShipmentId = shipment.ShipmentId,
				PackageId = shipment.PackageId.GetValueOrDefault(),
				IsStoppedInCustoms = shipment.IsStoppedInCustoms,
				Status = shipment.Status,
				ReceivedDate = shipment.ReceivedDate,
				CheckPoint = shipment.CheckPoint.ToEntity(),
				CreatedAt = shipment.CreatedAt,
				UpdatedAt = shipment.UpdatedAt
			};

			//Act
			var actualShipmentEntity = shipment.ToEntity();

			//Assert
			actualShipmentEntity
				.Should()
				.BeEquivalentTo(expected);
		}

		[TestMethod]
		public void ToEntity_WithNullDomain_ShouldReturnNull()
		{
			//Arrange
			var shipmentDomain = null as Shipment;

			//Act
			var actualShipmentEntity = shipmentDomain.ToEntity();

			//Assert
			actualShipmentEntity
				.Should()
				.BeNull();
		}
	}
}
