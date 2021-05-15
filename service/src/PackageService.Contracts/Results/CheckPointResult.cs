namespace PackageService.Contracts.Results
{
	using System;
	using PackageService.Shared.Enums;

	public class CheckPointResult
	{
		public CheckPointResult(
			string city,
			string country,
			ControlType controlType,
			PlaceType placeType,
			DateTime createdAt,
			DateTime? updatedAt)
		{
			City = city;
			Country = country;
			ControlType = controlType;
			PlaceType = placeType;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
		}

		public string City { get; }

		public string Country { get; }

		public ControlType ControlType { get; }

		public PlaceType PlaceType { get; }

		public DateTime CreatedAt { get; }

		public DateTime? UpdatedAt { get; }
	}
}
