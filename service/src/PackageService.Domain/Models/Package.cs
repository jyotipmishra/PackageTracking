namespace PackageService.Domain.Models
{
	using System;
	using System.Linq;
	using System.Text;
	using PackageService.Shared.Enums;

	public class Package
	{
		public static Package CreateNew(
			PackageSize size,
			int weight,
			bool isFragile,
			decimal? valueToPay,
			string countryCode,
			int countryISOCode,
			string areaCode,
			Shipment shipment)
		{
			return new Package(
				id: Guid.NewGuid(),
				size: size,
				weight: weight,
				isFragile: isFragile,
				valueToPay: valueToPay ?? 0,
				trackingCode: GenerateTrackingCode(
					countryCode: countryCode,
					countryISOCode: countryISOCode,
					areaCode: areaCode,
					receivedDate: shipment.ReceivedDate),
				shipment,
				null,
				createdAt: DateTime.UtcNow,
				updatedAt: null);
		}

		public Package(
			Guid id,
			PackageSize size,
			int weight,
			bool isFragile,
			decimal valueToPay,
			string trackingCode,
			Shipment shipment,
			Guid? version,
			DateTime createdAt,
			DateTime? updatedAt)
		{
			PackageId = id;
			Size = size;
			Weight = weight;
			IsFragile = isFragile;
			ValueToPay = valueToPay;
			TrackingCode = trackingCode;
			Shipment = shipment;
			Version = version;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
		}

		public Guid PackageId { get; }

		public PackageSize Size { get; }

		/// <summary>
		/// Weight in grams
		/// </summary>
		public int Weight { get; set; }

		public bool IsFragile { get; }

		public decimal ValueToPay { get; }

		public string TrackingCode { get; }

		public Shipment Shipment { get; }

		public DateTime CreatedAt { get; }

		public DateTime? UpdatedAt { get; }

		public Guid? Version { get; }

		#region Section to Generate Tracking Code
		private static string GenerateTrackingCode(
			string countryCode,
			int countryISOCode,
			string areaCode,
			DateTime receivedDate)
		{
			var trackingCode = new StringBuilder();

			var day = receivedDate.ToString("dd");
			var month = receivedDate.ToString("MM");
			var year = receivedDate.ToString("yy");

			trackingCode
				.Append(countryCode)
				.Append("-")
				.Append(areaCode.PadLeft(7, '0'))
				.Append("-")
				.Append(DateTime.Now.Ticks.ToString("x"))
				.Append("-")
				.Append(day).Append(month).Append(year)
				.Append("-")
				.Append(GetTOfZero(
					Array.ConvertAll(day.ToArray(), d => (int)d)[0],
					Array.ConvertAll(month.ToArray(), d => (int)d)[1]))
				.Append(GetTOfOne(
					countryISOCode,
					Array.ConvertAll(year.ToArray(), d => (int)d)[1]));

			return trackingCode.ToString();
		}

		private static int GetTOfZero(int dayOfZero, int monthOfOne)
		{
			return GetDigitSum((dayOfZero + monthOfOne + 20) * 2);
		}

		private static int GetTOfOne(int countryISOCode, int yearOfOne)
		{
			return GetDigitSum((countryISOCode + yearOfOne));
		}

		private static int GetDigitSum(int number)
		{
			if (number == 0)
				return 0;
			return (number % 9 == 0) 
				? 9 
				: (number % 9);
		}

		#endregion
	}
}
