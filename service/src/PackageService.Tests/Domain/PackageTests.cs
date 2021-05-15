namespace PackageService.Tests.Domain
{
	using System;
	using AutoFixture;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using PackageService.Domain.Models;
	using PackageService.Shared.Enums;

	[TestClass]
	public class PackageTests
	{
		private Fixture _fixture;

		[TestInitialize]
		public void TestInitialize()
		{
			_fixture = new Fixture();
		}

		[TestMethod]
		public void Package_CreateNew_ShouldReturnANewPackage()
		{
			//Arrange
			var packageSize = PackageSize.S;
			var weight = 100;
			var isFragile = false;
			decimal valueToPay = 200;
			var countryCode = "PT";
			var countryISOCode = 620;
			var areaCode = "Lis";
			var country = "Portugal";
			var city = "Lisbon";
			var controlType = ControlType.Custom;
			var placeType = PlaceType.CustomsFacility;

			//Act
			var package = Package.CreateNew(
				size: packageSize,
				weight: weight,
				isFragile: isFragile,
				valueToPay: valueToPay,
				countryCode: countryCode,
				countryISOCode: countryISOCode,
				areaCode: areaCode,
				shipment: Shipment.CreateNew(
					CheckPoint.CreateNew(
						city: city, 
						country: country, 
						controlType: controlType, 
						placeType: placeType)));

			//Assert
			Assert.AreNotEqual(Guid.Empty, package.PackageId);
			Assert.AreEqual(packageSize, package.Size);
			Assert.AreEqual(weight, package.Weight);
			Assert.AreEqual(isFragile, package.IsFragile);
			Assert.AreEqual(valueToPay, package.ValueToPay);
			Assert.IsTrue(!string.IsNullOrEmpty(package.TrackingCode));
			Assert.AreEqual(country, package.Shipment.CheckPoint.Country);
			Assert.AreEqual(city, package.Shipment.CheckPoint.City);
			Assert.AreEqual(controlType, package.Shipment.CheckPoint.ControlType);
			Assert.AreEqual(placeType, package.Shipment.CheckPoint.PlaceType);
		}
	}
}
