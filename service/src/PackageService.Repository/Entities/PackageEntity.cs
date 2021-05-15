namespace PackageService.Repository.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using PackageService.Shared.Enums;

	public class PackageEntity
	{
		[Key]
		public Guid PackageId { get; set; }

		[Required]
		public PackageSize Size { get; set; }

		/// <summary>
		/// Weight in grams
		/// </summary>
		[Required]
		public int Weight { get; set; }

		[Required]
		public bool IsFragile { get; set; }

		public decimal ValueToPay { get; set; }

		[Required]
		public string TrackingCode { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public Guid Version { get; set; }

		public virtual ShipmentEntity Shipment { get; set; }
	}
}
