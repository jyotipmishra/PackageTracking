namespace PackageService.Contracts.Parameters
{
	using System.ComponentModel.DataAnnotations;
	using PackageService.Shared.Enums;

	public class CheckPointParameters
	{
		[Required]
		public string Country { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public ControlType ControlType { get; set; }

		[Required]
		public PlaceType PlaceType { get; set; }
	}
}
