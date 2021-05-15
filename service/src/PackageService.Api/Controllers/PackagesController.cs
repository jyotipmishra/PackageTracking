namespace PackageService.Api.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using PackageService.Contracts.Mappers;
	using PackageService.Contracts.Parameters;
	using PackageService.Contracts.Results;
	using PackageService.Domain.Models;
	using PackageService.Repository.Repositories;
	using PackageService.Shared.Constants;
	using PackageService.Shared.Enums;
	using Syncfusion.XlsIO;

	[ApiController]
	[Route("api/[controller]")]
	public class PackagesController : ControllerBase
	{
		private readonly IPackageRepository _packageRepository;

		public PackagesController(IPackageRepository packageRepository)
		{
			_packageRepository = packageRepository
				?? throw new ArgumentNullException(nameof(packageRepository));
		}

		// POST	api/packages
		[HttpPost]
		[ProducesResponseType(typeof(void), 200)]
		public async Task<IActionResult> CreatePackages(CreatePackageParameters parameters)
		{
			if (!parameters.Packages.Any())
			{
				return BadRequest();
			}

			var packagesToCreate = parameters.Packages
				.Select(p =>
					Package.CreateNew(
						size: p.Size,
						weight: p.Weight,
						isFragile: p.IsFragile,
						valueToPay: p.ValueToPay,
					 	countryCode: p.CountryCode,
						countryISOCode: GetCountryCodeDetails(p.CountryCode),
						areaCode: p.AreaCode,
						shipment: Shipment.CreateNew(
							checkPoint: CheckPoint.CreateNew(
								city: p.CheckPointDetails.City,
								country: p.CheckPointDetails.Country,
								controlType: p.CheckPointDetails.ControlType,
								placeType: p.CheckPointDetails.PlaceType))));

			await _packageRepository.CreatePackages(packagesToCreate);

			return Ok();
		}

		// POST	api/packages/uploadexcel
		[HttpPost("uploadexcel")]
		[ProducesResponseType(typeof(void), 200)]
		public async Task<IActionResult> CreatePackagesFromExcel(IFormFile file)
		{
			var path = Path.Combine(Path.GetTempPath(), file.FileName);

			using (var stream = System.IO.File.Create(path))
			{
				await file.CopyToAsync(stream);
			}

			DataTable dataTable = null;

			using (Stream inputStream = System.IO.File.OpenRead(path))
			{
				using (ExcelEngine excelEngine = new ExcelEngine())
				{
					IApplication application = excelEngine.Excel;
					IWorkbook workbook = application.Workbooks.Open(inputStream);
					IWorksheet worksheet = workbook.Worksheets[0];

					dataTable = worksheet.ExportDataTable(
						worksheet.UsedRange,
						ExcelExportDataTableOptions.ColumnNames);
				}
			}

			List<Package> packagesToCreate = new List<Package>();

			for (var i = 0; i < dataTable.Rows.Count; i++)
			{
				packagesToCreate.Add(
				   Package.CreateNew(
					   size: (PackageSize)Convert.ToInt32(dataTable.Rows[i]["Package_Size"]),
					   weight: Convert.ToInt32(dataTable.Rows[i]["Package_Weight"]),
					   isFragile: Convert.ToBoolean(dataTable.Rows[i]["Package_IsFragile"]),
					   valueToPay: Convert.ToDecimal(dataTable.Rows[i]["Package_ValueToPay"]),
					   countryCode: dataTable.Rows[i]["Package_CountryCode"].ToString(),
					   countryISOCode: GetCountryCodeDetails(dataTable.Rows[i]["Package_CountryCode"].ToString()),
					   areaCode: dataTable.Rows[i]["Package_AreaCode"].ToString(),
					   shipment: Shipment.CreateNew(
						checkPoint: CheckPoint.CreateNew(
							city: dataTable.Rows[i]["CheckPoint_City"].ToString(),
							country: dataTable.Rows[i]["CheckPoint_Country"].ToString(),
							controlType: (ControlType)Convert.ToInt32(dataTable.Rows[i]["CheckPoint_ControlType"]),
							placeType: (PlaceType)Convert.ToInt32(dataTable.Rows[i]["CheckPoint_PlaceType"])))));
			}

			await _packageRepository.CreatePackages(packagesToCreate);

			return Ok();
		}

		// GET api/packages/tracking/{trackingcode}
		[HttpGet("tracking/{code}")]
		[ProducesResponseType(typeof(PackageResult), 200)]
		public async Task<IActionResult> GetPackageByTrackingCode(string code)
		{
			if (string.IsNullOrEmpty(code))
			{
				return BadRequest();
			}

			var package = await _packageRepository
				.GetPackageByTrackingCode(code.Trim());

			if (package == null)
			{
				return NotFound();
			}

			return Ok(package.ToResult());
		}

		// GET api/packages/status/{statuscode}
		[HttpGet("status/{code}")]
		[ProducesResponseType(typeof(PackagesPagedResult), 200)]
		public async Task<IActionResult> GetPackagesByStatusCode(
			int code,
			int pageNumber = 0,
			int itemsPerPage = 50)
		{
			if (!Enum.IsDefined(typeof(ShipmentStatus), code))
			{
				return BadRequest("Invalid status code");
			}

			var packages = await _packageRepository.GetPackagesByStatusCode(
				statusCode: (ShipmentStatus)code,
				page: pageNumber,
				pageSize: itemsPerPage);

			if (packages == null || packages.List.Count() == 0)
			{
				return NotFound();
			}

			var returnedResult = new PackagesPagedResult(
				packages
					.List
					.Select(model => model.ToResult()),
				packages.TotalCount);

			return Ok(returnedResult);
		}

		// GET api/packages/place/{placetype}
		[HttpGet("place/{type}")]
		[ProducesResponseType(typeof(PackagesPagedResult), 200)]
		public async Task<IActionResult> GetPackagesByPlaceType(
			int type,
			int pageNumber = 0,
			int itemsPerPage = 50)
		{
			if (!Enum.IsDefined(typeof(PlaceType), type))
			{
				return BadRequest("Invalid status code");
			}

			var packages = await _packageRepository.GetPackagesByPlaceType(
				type: (PlaceType)type,
				page: pageNumber,
				pageSize: itemsPerPage);

			if (packages == null || packages.List.Count() == 0)
			{
				return NotFound();
			}

			var returnedResult = new PackagesPagedResult(
				packages
					.List
					.Select(model => model.ToResult()),
				packages.TotalCount);

			return Ok(returnedResult);
		}

		// PUT api/packages/{trackingCode}
		[HttpPut("{trackingCode}")]
		[ProducesResponseType(typeof(void), 200)]
		public async Task<IActionResult> UpdatePackage(
			string trackingCode,
			UpdatePackageParameters parameters)
		{
			if (string.IsNullOrEmpty(trackingCode))
			{
				return BadRequest();
			}

			var package = await _packageRepository
				.GetPackageByTrackingCode(trackingCode.Trim());

			if (package == null)
			{
				return NotFound();
			}

			if (package.Version != parameters.Version)
			{
				return Conflict(new { message = "An Attempt was made to update with old data." });
			}

			package.Shipment.Update(
				parameters.IsStoppedInCustoms,
				parameters.Status);

			package.Shipment.CheckPoint.Update(
				city: parameters.City,
				country: parameters.Country,
				controlType: parameters.ControlType,
				placeType: parameters.PlaceType);

			await _packageRepository.UpdatePackage(package);

			return Ok();
		}

		#region Private methods
		private int GetCountryCodeDetails(string countryCode)
		{
			if (CountryCodes.GetCountryCodes().ContainsKey(countryCode.ToUpper()))
			{
				return CountryCodes.GetCountryCodes()[countryCode.ToUpper()];
			}
			else
			{
				throw new Exception("Invalid country code");
			}
		}
		#endregion
	}
}
