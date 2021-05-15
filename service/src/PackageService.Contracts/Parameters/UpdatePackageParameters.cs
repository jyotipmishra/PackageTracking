namespace PackageService.Contracts.Parameters
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using PackageService.Shared.Enums;

	public class UpdatePackageParameters
	{
		public bool? IsStoppedInCustoms { get; set; }			                                                                                  

		public ShipmentStatus? Status { get; set; }

		public string Country { get; set; }

		public string City { get; set; }

		public ControlType? ControlType { get; set; }

		public PlaceType? PlaceType { get; set; }

		[Required]
		public Guid Version { get; set; }
	}															                    
}
