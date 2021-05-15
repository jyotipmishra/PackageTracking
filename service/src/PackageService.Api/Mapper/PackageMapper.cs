namespace PackageService.Contracts.Mappers
{
	using PackageService.Contracts.Results;
	using PackageService.Domain.Models;

	public static class PackageMapper
	{
		public static PackageResult ToResult(this Package package)
		{
			return package == null
			   ? null
			   : new PackageResult(
					size: package.Size,
					weight: package.Weight,
					isFragile: package.IsFragile,
					valueToPay: package.ValueToPay,
					trackingCode: package.TrackingCode,
					shipmentResult: package.Shipment.ToResult(),
					createdAt: package.CreatedAt,
					version: package.Version.GetValueOrDefault());
		}
	}
}
