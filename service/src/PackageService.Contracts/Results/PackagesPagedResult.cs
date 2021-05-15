namespace PackageService.Contracts.Results
{
	using System.Collections.Generic;
	using System.Linq;
	using PackageService.Shared.Api;

	public class PackagesPagedResult : GetPagedResult<PackageResult>
	{
		public PackagesPagedResult(IEnumerable<PackageResult> packages, int totalCount)
			: base(packages, totalCount)
		{
			TotalValueToPay = packages.Select(a => a.ValueToPay).Sum();
		}

		public decimal TotalValueToPay { get; }
	}
}
