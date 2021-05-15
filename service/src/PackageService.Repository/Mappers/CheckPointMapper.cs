namespace PackageService.Repository.Mappers
{
	using PackageService.Domain.Models;
	using PackageService.Repository.Entities;

	public static class CheckPointMapper
	{
		public static CheckPointEntity ToEntity(this CheckPoint checkPoint)
		{
			return checkPoint == null
				? null
				: new CheckPointEntity
				{
					CheckPointId = checkPoint.CheckPointId,
					ShipmentId = checkPoint.ShipmentId.GetValueOrDefault(),
					City = checkPoint.City,
					Country = checkPoint.Country,
					ControlType = checkPoint.ControlType,
					PlaceType = checkPoint.PlaceType,
					CreatedAt = checkPoint.CreatedAt,
					UpdatedAt = checkPoint.UpdatedAt
				}; 
		}

		public static CheckPoint ToDomain(this CheckPointEntity checkPoint)
		{
			return checkPoint == null
				? null
				: new CheckPoint(
					checkPointId: checkPoint.CheckPointId, 
					shipmentId: checkPoint.ShipmentId, 
				 	city: checkPoint.City, 
					country: checkPoint.Country, 
					controlType: checkPoint.ControlType, 
					placeType: checkPoint.PlaceType,
					createdAt: checkPoint.CreatedAt,
					updatedAt: checkPoint.UpdatedAt);
		}
	}
}
