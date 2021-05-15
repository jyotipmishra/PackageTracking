namespace PackageService.Domain.Models
{
	using System;
	using PackageService.Shared.Enums;

	public class CheckPoint
	{
		public static CheckPoint CreateNew(
			string city,
			string country,
			ControlType controlType,
			PlaceType placeType)
		{
			return new CheckPoint(
				checkPointId: Guid.NewGuid(),
				shipmentId: null,
				city: city,
				country: country,
				controlType: controlType,
				placeType: placeType,
				createdAt: DateTime.UtcNow,
				updatedAt: null);
		}

		public CheckPoint(
			Guid checkPointId,
			Guid? shipmentId,
			string city,
			string country,
			ControlType controlType,
			PlaceType placeType,
			DateTime createdAt,
			DateTime? updatedAt)
		{
			CheckPointId = checkPointId;
			ShipmentId = shipmentId;
			City = city;
			Country = country;
			ControlType = controlType;
			PlaceType = placeType;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
		}

		public Guid CheckPointId { get; }

		public Guid? ShipmentId { get; }

		public string City { get; private set; }

		public string Country { get; private set; }

		public ControlType ControlType { get; private set; }

		public PlaceType PlaceType { get; private set; }

		public DateTime CreatedAt { get; }

		public DateTime? UpdatedAt { get; private set; }

		public void Update(
			string city,
			string country,
			ControlType? controlType,
			PlaceType? placeType)
		{
			bool isUpdated = false;

			if (!string.IsNullOrEmpty(city))
			{
				City = city;
				isUpdated = true;
			}

			if (!string.IsNullOrEmpty(country))
			{
				Country = country;
				isUpdated = true;
			}

			if (controlType.HasValue && Enum.IsDefined(typeof(ControlType), controlType))
			{
				ControlType = controlType.Value;
				isUpdated = true;
			}

			if (placeType.HasValue && Enum.IsDefined(typeof(PlaceType), placeType))
			{
				PlaceType = placeType.Value;
				isUpdated = true;
			}

			if (isUpdated)
			{
				UpdatedAt = DateTime.UtcNow;
			}
		}
	}
}
