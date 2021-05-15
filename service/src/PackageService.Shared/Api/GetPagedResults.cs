namespace PackageService.Shared.Api
{
	using System.Collections.Generic;
	using System.Linq;

	public class GetPagedResult<T>
	{
		public IEnumerable<T> List
		{
			get;
		}

		public int TotalCount
		{
			get;
		}

		public GetPagedResult(IEnumerable<T> list, int totalCount)
		{
			List = (list ?? Enumerable.Empty<T>());
			TotalCount = totalCount;
		}
	}
}
