namespace PackageService.Domain.Models
{
	using System;
	using PackageService.Shared.Enums;

	public class Shipment
	{
		public static Shipment CreateNew(CheckPoint checkPoint)
		{
			return new Shipment(
				Guid.NewGuid(),
				null,
				false,
				ShipmentStatus.Received,
				DateTime.UtcNow,
				checkPoint,
				DateTime.UtcNow,
				null);
		}

		public Shipment(
			Guid shipmentId,
			Guid? packageId,
			bool isStoppedInCustom,
			ShipmentStatus status,
			DateTime receivedDate,
			CheckPoint checkPoint,
			DateTime createdAt,
			DateTime? updatedAt)
		{
			ShipmentId = shipmentId;
			PackageId = packageId;
			IsStoppedInCustoms = isStoppedInCustom;
			Status = status;
			ReceivedDate = receivedDate;
			CheckPoint = checkPoint;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
		}

		public Guid ShipmentId { get; }

		public Guid? PackageId { get; }

		public bool IsStoppedInCustoms { get; private set; }

		public ShipmentStatus Status { get; private set; }

		public DateTime ReceivedDate { get; set; }

		public DateTime CreatedAt { get; }

		public DateTime? UpdatedAt { get; private set; }

		public CheckPoint CheckPoint { get; }

		public void Update(bool? isStoppedInCustoms, ShipmentStatus? status)
		{
			bool isUpdated = false;

			if (isStoppedInCustoms.HasValue)
			{
				IsStoppedInCustoms = isStoppedInCustoms.Value;
				isUpdated = true;
			}

			if (status.HasValue && Enum.IsDefined(typeof(ShipmentStatus), status))
			{
				Status = status.Value;
				isUpdated = true;
			}

			if (isUpdated)
			{
				UpdatedAt = DateTime.UtcNow;
			}
		}
	}
}
