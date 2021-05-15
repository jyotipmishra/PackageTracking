namespace PackageService.Contracts.Mappers
{
	using PackageService.Contracts.Results;
	using PackageService.Domain.Models;

	public static class CheckPointMapper
	{
		public static CheckPointResult ToResult(this CheckPoint checkPoint)
		{
			return checkPoint == null
				? null
				: new CheckPointResult(
					city: checkPoint.City, 
					country: checkPoint.Country, 
					controlType: checkPoint.ControlType, 
					placeType: checkPoint.PlaceType,
					createdAt: checkPoint.CreatedAt,
					updatedAt: checkPoint.UpdatedAt); 
		}
	}
}
