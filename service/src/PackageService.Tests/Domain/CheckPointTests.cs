namespace PackageService.Tests.Domain
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using PackageService.Domain.Models;
	using PackageService.Shared.Enums;

	[TestClass]
	public class CheckPointTests
	{
		[TestMethod]
		public void CheckPoint_CreateNew_ShouldReturnANewCheckPoint()
		{
			//Arrange
			var country = "Portugal";
			var city = "Lisbon";
			var controlType = ControlType.Custom;
			var placeType = PlaceType.CustomsFacility;

			//Act
			var checkPoint = CheckPoint.CreateNew(
						city: city,
						country: country,
						controlType: controlType,
						placeType: placeType);

			//Assert
			Assert.AreNotEqual(Guid.Empty, checkPoint.CheckPointId);
			Assert.AreEqual(country, checkPoint.Country);
			Assert.AreEqual(city, checkPoint.City);
			Assert.AreEqual(controlType, checkPoint.ControlType);
			Assert.AreEqual(placeType, checkPoint.PlaceType);
		}
	}
}
