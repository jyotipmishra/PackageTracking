namespace PackageService.Contracts.Parameters
{
	using System.ComponentModel.DataAnnotations;
	using PackageService.Shared.Enums;

	public class PackageParameters
	{
		[Required]
		[StringLength(2, MinimumLength = 2)]
		public string CountryCode { get; set; }

		[Required]
		[StringLength(7, MinimumLength = 1)]
		public string AreaCode { get; set; }

		[Required]
		public PackageSize Size { get; set; }

		[Required]
		public int Weight { get; set; }

		[Required]
		public bool IsFragile { get; set; }

		[Required]
		public CheckPointParameters CheckPointDetails { get; set; }

		public decimal? ValueToPay { get; set; }
	}
}
