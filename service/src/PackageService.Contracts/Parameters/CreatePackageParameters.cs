namespace PackageService.Contracts.Parameters
{
	using System.Collections.Generic;

	public class CreatePackageParameters
	{
		public IEnumerable<PackageParameters> Packages { get; set; }
	}
}
