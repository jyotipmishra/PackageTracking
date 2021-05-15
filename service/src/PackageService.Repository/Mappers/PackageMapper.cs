namespace PackageService.Repository.Mappers
{
	using System;
	using PackageService.Domain.Models;
	using PackageService.Repository.Entities;

	public static class PackageMapper
	{
		public static PackageEntity ToEntity(this Package package)
		{
			 return package == null
				? null
				: new PackageEntity
				{
					PackageId = package.PackageId,
					Size = package.Size,
					Weight = package.Weight,
					IsFragile = package.IsFragile,
					ValueToPay = package.ValueToPay,
					TrackingCode = package.TrackingCode,
					CreatedAt = package.CreatedAt,
					UpdatedAt = package.UpdatedAt,
					Shipment = package.Shipment.ToEntity(),
					Version = Guid.NewGuid()
				};
		}

		public static Package ToDomain(this PackageEntity package)
		{
			return package == null
			   ? null
			   : new Package(
				   id: package.PackageId, 
				   size: package.Size, 
				   weight: package.Weight, 
				   isFragile: package.IsFragile, 
				   valueToPay: package.ValueToPay, 
				   trackingCode: package.TrackingCode,
				   shipment: package.Shipment.ToDomain(),
				   version: package.Version,
				   createdAt: package.CreatedAt,
				   updatedAt: package.UpdatedAt);
		}
	}
}
