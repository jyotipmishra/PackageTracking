namespace PackageService.Contracts.Results
{
	using System;
	using PackageService.Shared.Enums;

	public class ShipmentResult
	{
		public ShipmentResult(
			bool isStoppedInCustom,
			ShipmentStatus status,
			DateTime receivedDate,
			CheckPointResult checkPointResult,
			DateTime createdAt,
			DateTime? updatedAt)
		{
			IsStoppedInCustoms = isStoppedInCustom;
			Status = status;
			ReceivedDate = receivedDate;
			CheckPointResult = checkPointResult;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
		}

		public bool IsStoppedInCustoms { get; }

		public ShipmentStatus Status { get; }

		public DateTime ReceivedDate { get; set; }

		public DateTime CreatedAt { get; }

		public DateTime? UpdatedAt { get; }

		public CheckPointResult CheckPointResult { get; }
	}
}
