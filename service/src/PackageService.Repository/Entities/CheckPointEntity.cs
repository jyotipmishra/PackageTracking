namespace PackageService.Repository.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using PackageService.Shared.Enums;

	public class CheckPointEntity
	{
		[Key]
		public Guid CheckPointId { get; set; }

		[Required]
		public Guid ShipmentId { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string Country { get; set; }

		[Required]
		public ControlType ControlType { get; set; }

		[Required]
		public PlaceType PlaceType { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		[ForeignKey(nameof(ShipmentId))]
		public virtual ShipmentEntity Shipment { get; set; }
	}
}
