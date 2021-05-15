namespace PackageService.Contracts.Mappers
{
	using PackageService.Contracts.Results;
	using PackageService.Domain.Models;

	public static class ShipmentMapper
	{
		public static ShipmentResult ToResult(this Shipment shipment)
		{
			return shipment == null 
				? null 
				: new ShipmentResult(shipment.IsStoppedInCustoms,
					status: shipment.Status,
					receivedDate: shipment.ReceivedDate,
					checkPointResult: shipment.CheckPoint.ToResult(),
					createdAt: shipment.CreatedAt,
					updatedAt: shipment.UpdatedAt);
		}
	}
}
