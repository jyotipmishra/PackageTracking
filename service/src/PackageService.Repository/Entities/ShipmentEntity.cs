namespace PackageService.Repository.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using PackageService.Shared.Enums;

	public class ShipmentEntity
	{
		[Key]
		public Guid ShipmentId { get; set; }

		[Required]
		public Guid	PackageId { get; set; }

		[Required]
		public bool IsStoppedInCustoms { get; set; }

		[Required]
		public ShipmentStatus Status { get; set; }

		[Required]
		public DateTime ReceivedDate { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		[ForeignKey(nameof(PackageId))]
		public virtual PackageEntity Package { get; set; }

		public virtual CheckPointEntity CheckPoint { get; set; }
	}
}
