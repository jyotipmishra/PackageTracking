namespace PackageService.Repository.Mappers
{
	using PackageService.Domain.Models;
	using PackageService.Repository.Entities;
	public static class ShipmentMapper
	{
		public static ShipmentEntity ToEntity(this Shipment shipment)
		{
			return shipment == null 
				? null 
				: new ShipmentEntity
				{
					ShipmentId = shipment.ShipmentId,
					PackageId = shipment.PackageId.GetValueOrDefault(),
					IsStoppedInCustoms = shipment.IsStoppedInCustoms,
					Status = shipment.Status,
					ReceivedDate = shipment.ReceivedDate,
					CheckPoint = shipment.CheckPoint.ToEntity(),
					CreatedAt = shipment.CreatedAt,
					UpdatedAt = shipment.UpdatedAt
				};
		}

		public static Shipment ToDomain(this ShipmentEntity shipment)
		{
			return shipment == null
				? null
				: new Shipment(
					shipmentId: shipment.ShipmentId, 
					packageId: shipment.PackageId, 
					isStoppedInCustom: shipment.IsStoppedInCustoms, 
					status: shipment.Status, 
					receivedDate: shipment.ReceivedDate,
					checkPoint: shipment.CheckPoint.ToDomain(),
					createdAt: shipment.CreatedAt,
					updatedAt: shipment.UpdatedAt);
		}
	}
}
