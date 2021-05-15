namespace PackageService.Tests.Api
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Net;
	using System.Threading.Tasks;
	using AutoFixture;
	using FluentAssertions;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using PackageService.Api.Controllers;
	using PackageService.Contracts.Parameters;
	using PackageService.Contracts.Results;
	using PackageService.Domain.Models;
	using PackageService.Repository.Repositories;
	using PackageService.Shared.Api;
	using PackageService.Shared.Enums;

	[TestClass]
	public class PackagesControllerTests
	{
		private static PackagesController _packagesController;
		private Mock<IPackageRepository> _mockPackageRepository;
		private Mock<IFormFile> _file;
		private Fixture _fixture;

		[TestInitialize]
		public void TestInitialize()
		{
			_mockPackageRepository = new Mock<IPackageRepository>(MockBehavior.Strict);

			_fixture = new Fixture();

			_packagesController
				= new PackagesController(_mockPackageRepository.Object);

			_file = new Mock<IFormFile>();
		}

		[TestMethod]
		public async Task CreatePackages_WithValidData_PackagesCreatedSuccessfully()
		{
			//Arrange
			var packageParameters = _fixture
			   .Build<PackageParameters>()
			   .With(a => a.CountryCode, "PT")
			   .With(a => a.AreaCode, "LB")
			   .CreateMany(100);

			var parameters = new CreatePackageParameters
			{
				Packages = packageParameters
			};

			_mockPackageRepository
				.Setup(exec =>
				exec.CreatePackages(
					It.IsAny<IEnumerable<Package>>()))
				.Returns(Task.CompletedTask);

			//Act
			var response = await _packagesController
				.CreatePackages(parameters) as OkResult;

			//Assert
			_mockPackageRepository
				.Verify(exec =>
				exec.CreatePackages(
					It.IsAny<IEnumerable<Package>>()),
					Times.Once);

			response.StatusCode
				.Should()
				.Be((int)HttpStatusCode.OK);
		}

		[TestMethod]
		public async Task CreatePackages_WithoutValidParameters_ReturnsbadRequest()
		{
			//Arrange
			var packageParameters = _fixture
			   .Build<PackageParameters>()
			   .With(a => a.CountryCode, "PT")
			   .With(a => a.AreaCode, "LB")
			   .CreateMany(0);

			var parameters = new CreatePackageParameters
			{
				Packages = packageParameters
			};

			_mockPackageRepository
				.Setup(exec =>
				exec.CreatePackages(
					It.IsAny<IEnumerable<Package>>()))
				.Returns(Task.CompletedTask);

			//Act
			var response = await _packagesController
				.CreatePackages(parameters) as BadRequestResult;

			//Assert
			_mockPackageRepository
				.Verify(exec =>
				exec.CreatePackages(
					It.IsAny<IEnumerable<Package>>()),
					Times.Never);

			response.StatusCode
				.Should()
				.Be((int)HttpStatusCode.BadRequest);
		}

		[TestMethod]
		public async Task CreatePackagesFromExcel_WithValidParameters_CreatesPackagesSuccessfully()
		{
			//Arrange
			string currentDirectory = Directory.GetCurrentDirectory();
			string filePath = Path.Combine(currentDirectory, "Api", "PackageInputs.xlsx");

			var content = File.OpenRead(filePath);

			var file = new FormFile(
				content,
				0,
				content.Length,
				null,
				Path.GetFileName(filePath))
			{
				Headers = new HeaderDictionary(),
				ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
			};

			_file.Setup(exec => exec.OpenReadStream())
				.Returns(content);

			_mockPackageRepository
				.Setup(exec =>
				exec.CreatePackages(
					It.IsAny<IEnumerable<Package>>()))
				.Returns(Task.CompletedTask);

			//Act
			var response = await _packagesController
				.CreatePackagesFromExcel(file) as OkResult;

			//Assert
			_mockPackageRepository
				.Verify(exec =>
				exec.CreatePackages(
					It.IsAny<IEnumerable<Package>>()),
					Times.Once);

			response.StatusCode
				.Should()
				.Be((int)HttpStatusCode.OK);
		}

		[TestMethod]
		public async Task GetPackageByTrackingCode_WithValidTrackingCode_ReturnsPackageSuccessfully()
		{
			//Arrange
			var trackingCode = _fixture.Create<string>();

			var expectedResult = new Package(
				Guid.NewGuid(),
				Shared.Enums.PackageSize.M,
				100,
				false,
				200,
				trackingCode,
				_fixture.Create<Shipment>(),
				null,
				DateTime.UtcNow,
				null);

			_mockPackageRepository
				.Setup(exec =>
				exec.GetPackageByTrackingCode(
					It.IsAny<string>()))
				.ReturnsAsync(expectedResult);

			//Act
			var response = await _packagesController
				.GetPackageByTrackingCode(trackingCode) as OkObjectResult;

			//Assert
			_mockPackageRepository
				.Verify(exec =>
				exec.GetPackageByTrackingCode(
					It.IsAny<string>()),
					Times.Once);

			var packageResult = response.Value as PackageResult;

			response.StatusCode
				.Should()
				.Be((int)HttpStatusCode.OK);

			packageResult.TrackingCode
				.Should()
				.Be(expectedResult.TrackingCode);
		}

		[TestMethod]
		public async Task GetPackageByTrackingCode_WithInvalidTrackingCode_ReturnsNotFound()
		{
			//Arrange
			var expectedResult = _fixture.Create<Package>();

			_mockPackageRepository
				.Setup(exec =>
				exec.GetPackageByTrackingCode(
					It.IsAny<string>()))
				.ReturnsAsync((Package) null);

			//Act
			var response = await _packagesController
				.GetPackageByTrackingCode(_fixture.Create<string>()) as NotFoundResult;

			//Assert
			_mockPackageRepository
				.Verify(exec =>
				exec.GetPackageByTrackingCode(
					It.IsAny<string>()),
					Times.Once);

			response.StatusCode
				.Should()
				.Be((int)HttpStatusCode.NotFound);
		}

		[TestMethod]
		public async Task GetPackagesByStatusCode_WithValidStatusCode_ReturnsPackagesSuccessfully()
		{
			//Arrange
			var expectedResult = _fixture.CreateMany<Package>(50);

			var pagedResult = new GetPagedResult<Package>(expectedResult, 50);

			_mockPackageRepository
				.Setup(exec =>
				exec.GetPackagesByStatusCode(
					It.IsAny<ShipmentStatus>(),
					0,
					50))
				.ReturnsAsync(pagedResult);

			//Act
			var response = await _packagesController
				.GetPackagesByStatusCode(0) as OkObjectResult;

			//Assert
			_mockPackageRepository
				.Verify(exec =>
				exec.GetPackagesByStatusCode(
					It.IsAny<ShipmentStatus>(),
					0,
					50),
					Times.Once);

			response.StatusCode
				.Should()
				.Be((int)HttpStatusCode.OK);
		}

		[TestMethod]
		public async Task GetPackagesByStatusCode_WithInvalidStatusCode_ReturnsNotFound()
		{
			//Arrange
			var expectedResult = _fixture.CreateMany<Package>(0);

			var pagedResult = new GetPagedResult<Package>(expectedResult, 0);

			_mockPackageRepository
				.Setup(exec =>
				exec.GetPackagesByStatusCode(
					It.IsAny<ShipmentStatus>(),
					0,
					50))
				.ReturnsAsync(pagedResult);

			//Act
			var response = await _packagesController
				.GetPackagesByStatusCode(3) as NotFoundResult;

			//Assert
			_mockPackageRepository
				.Verify(exec =>
				exec.GetPackagesByStatusCode(
					It.IsAny<ShipmentStatus>(),
					0,
					50),
					Times.Once);

			response.StatusCode
				.Should()
				.Be((int)HttpStatusCode.NotFound);
		}

		[TestMethod]
		public async Task GetPackagesByPlaceType_WithValidType_ReturnsPackagesSuccessfully()
		{
			//Arrange
			var expectedResult = _fixture.CreateMany<Package>(50);

			var pagedResult = new GetPagedResult<Package>(expectedResult, 50);

			_mockPackageRepository
				.Setup(exec =>
				exec.GetPackagesByPlaceType(
					It.IsAny<PlaceType>(),
					0,
					50))
				.ReturnsAsync(pagedResult);

			//Act
			var response = await _packagesController
				.GetPackagesByPlaceType(0) as OkObjectResult;

			//Assert
			_mockPackageRepository
				.Verify(exec =>
				exec.GetPackagesByPlaceType(
					It.IsAny<PlaceType>(),
					0,
					50),
					Times.Once);

			response.StatusCode
				.Should()
				.Be((int)HttpStatusCode.OK);
		}

		[TestMethod]
		public async Task GetPackagesByPlaceType_WithInvalidType_ReturnsNotFound()
		{
			//Arrange
			var expectedResult = _fixture.CreateMany<Package>(0);

			var pagedResult = new GetPagedResult<Package>(expectedResult, 0);

			_mockPackageRepository
				.Setup(exec =>
				exec.GetPackagesByPlaceType(
					It.IsAny<PlaceType>(),
					0,
					50))
				.ReturnsAsync(pagedResult);

			//Act
			var response = await _packagesController
				.GetPackagesByPlaceType(3) as NotFoundResult;

			//Assert
			_mockPackageRepository
				.Verify(exec =>
				exec.GetPackagesByPlaceType(
					It.IsAny<PlaceType>(),
					0,
					50),
					Times.Once);

			response.StatusCode
				.Should()
				.Be((int)HttpStatusCode.NotFound);
		}

		[TestMethod]
		public async Task UpdatePackage_WithValidData_PackagesUpdatedSuccessfully()
		{
			//Arrange
			var version = Guid.NewGuid();

			var packageParameters = _fixture
			   .Build<UpdatePackageParameters>()
			   .With(a => a.Status, ShipmentStatus.Delivered)
			   .With(a => a.Version, version)
			   .Create();

			var trackingCode = _fixture.Create<string>();

			var expectedResult = new Package(
				Guid.NewGuid(),
				PackageSize.M,
				100,
				false,
				200,
				trackingCode,
				_fixture.Create<Shipment>(),
				version,
				DateTime.UtcNow,
				null);

			_mockPackageRepository
				.Setup(exec =>
				exec.GetPackageByTrackingCode(
					It.IsAny<string>()))
				.ReturnsAsync(expectedResult);

			_mockPackageRepository
				.Setup(exec =>
				exec.UpdatePackage(
					It.IsAny<Package>()))
				.Returns(Task.CompletedTask);

			//Act
			var response = await _packagesController
				.UpdatePackage(trackingCode, packageParameters) as OkResult;

			//Assert
			_mockPackageRepository
				.Verify(exec =>
				exec.GetPackageByTrackingCode(
					It.IsAny<string>()),
					Times.Once);

			_mockPackageRepository
				.Verify(exec =>
				exec.UpdatePackage(
					It.IsAny<Package>()),
					Times.Once);

			response.StatusCode
				.Should()
				.Be((int)HttpStatusCode.OK);
		}

		[TestMethod]
		public async Task UpdatePackage_WithEmptyTrackingCode_ReturnsBadRequest()
		{
			//Arrange
			var packageParameters = _fixture
			   .Build<UpdatePackageParameters>()
			   .With(a => a.Status, ShipmentStatus.Delivered)
			   .Create();

			var trackingCode = _fixture.Create<string>();

			var expectedResult = new Package(
				Guid.NewGuid(),
				PackageSize.M,
				100,
				false,
				200,
				trackingCode,
				_fixture.Create<Shipment>(),
				null,
				DateTime.UtcNow,
				null);

			_mockPackageRepository
				.Setup(exec =>
				exec.GetPackageByTrackingCode(
					It.IsAny<string>()))
				.ReturnsAsync(expectedResult);

			_mockPackageRepository
				.Setup(exec =>
				exec.UpdatePackage(
					It.IsAny<Package>()))
				.Returns(Task.CompletedTask);

			//Act
			var response = await _packagesController
				.UpdatePackage("", packageParameters) as BadRequestResult;

			//Assert
			_mockPackageRepository
				.Verify(exec =>
				exec.GetPackageByTrackingCode(
					It.IsAny<string>()),
					Times.Never);

			_mockPackageRepository
				.Verify(exec =>
				exec.UpdatePackage(
					It.IsAny<Package>()),
					Times.Never);

			response.StatusCode
				.Should()
				.Be((int)HttpStatusCode.BadRequest);
		}

		[TestMethod]
		public async Task UpdatePackage_WithInvalidTrackingCode_ReturnsNotFound()
		{
			//Arrange
			var packageParameters = _fixture
			   .Build<UpdatePackageParameters>()
			   .With(a => a.Status, ShipmentStatus.Delivered)
			   .Create();

			_mockPackageRepository
				.Setup(exec =>
				exec.GetPackageByTrackingCode(
					It.IsAny<string>()))
				.ReturnsAsync((Package) null);

			//Act
			var response = await _packagesController
				.UpdatePackage(_fixture.Create<string>(), packageParameters) as NotFoundResult;

			//Assert
			_mockPackageRepository
				.Verify(exec =>
				exec.GetPackageByTrackingCode(
					It.IsAny<string>()),
					Times.Once);

			_mockPackageRepository
				.Verify(exec =>
				exec.UpdatePackage(
					It.IsAny<Package>()),
					Times.Never);

			response.StatusCode
				.Should()
				.Be((int)HttpStatusCode.NotFound);
		}

		[TestMethod]
		public async Task UpdatePackage_WithOlderVersionData_ReturnsConflict()
		{
			//Arrange
			var packageParameters = _fixture
			   .Build<UpdatePackageParameters>()
			   .With(a => a.Status, ShipmentStatus.Delivered)
			   .Create();

			var expectedResult = new Package(
				Guid.NewGuid(),
				PackageSize.M,
				100,
				false,
				200,
				_fixture.Create<string>(),
				_fixture.Create<Shipment>(),
				null,
				DateTime.UtcNow,
				null);

			_mockPackageRepository
				.Setup(exec =>
				exec.GetPackageByTrackingCode(
					It.IsAny<string>()))
				.ReturnsAsync(expectedResult);

			//Act
			var response = await _packagesController
				.UpdatePackage(_fixture.Create<string>(), packageParameters) as ConflictObjectResult;

			//Assert
			_mockPackageRepository
				.Verify(exec =>
				exec.GetPackageByTrackingCode(
					It.IsAny<string>()),
					Times.Once);

			_mockPackageRepository
				.Verify(exec =>
				exec.UpdatePackage(
					It.IsAny<Package>()),
					Times.Never);

			response.StatusCode
				.Should()
				.Be((int)HttpStatusCode.Conflict);
		}
	}
}
