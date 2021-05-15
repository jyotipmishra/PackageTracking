namespace PackageService.Contracts.Results
{
	using System;
	using PackageService.Shared.Enums;

	public class PackageResult
	{
		public PackageResult(
			PackageSize size,
			int weight,
			bool isFragile,
			decimal valueToPay,
			string trackingCode,
			ShipmentResult shipmentResult,
			DateTime createdAt,
			Guid version)
		{
			Size = size;
			Weight = weight;
			IsFragile = isFragile;
			ValueToPay = valueToPay;
			TrackingCode = trackingCode;
			ShipmentResult = shipmentResult;
			CreatedAt = createdAt;
			Version = version;
		}

		public PackageSize Size { get; }

		public int Weight { get; }

		public bool IsFragile { get; }

		public decimal ValueToPay { get; }

		public string CountryCode { get; }

		public string TrackingCode { get; }

		public DateTime CreatedAt { get; }

		public ShipmentResult ShipmentResult { get; }

		public Guid Version { get; }
	}
}
