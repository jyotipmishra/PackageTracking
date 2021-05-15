namespace PackageService.Tests.Domain
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using PackageService.Domain.Models;
	using PackageService.Shared.Enums;

	[TestClass]
	public class ShipmentTests
	{
		[TestMethod]
		public void Shipment_CreateNew_ShouldReturnANewShipment()
		{
			//Arrange
			var country = "Portugal";
			var city = "Lisbon";
			var controlType = ControlType.Custom;
			var placeType = PlaceType.CustomsFacility;

			//Act
			var shipment = Shipment.CreateNew(
					CheckPoint.CreateNew(
						city: city,
						country: country,
						controlType: controlType,
						placeType: placeType));

			//Assert
			Assert.AreNotEqual(Guid.Empty, shipment.ShipmentId);
			Assert.AreEqual(country, shipment.CheckPoint.Country);
			Assert.AreEqual(city, shipment.CheckPoint.City);
			Assert.AreEqual(controlType, shipment.CheckPoint.ControlType);
			Assert.AreEqual(placeType, shipment.CheckPoint.PlaceType);
		}
	}
}
