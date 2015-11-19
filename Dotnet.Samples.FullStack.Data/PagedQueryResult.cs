using System.Collections.Generic;

namespace Dotnet.Samples.FullStack.Data
{
	public class PagedQueryResult<TEntity>
	{
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		public int TotalPages { get; set; }
		public long TotalItems { get; set; }
		public IList<TEntity> Items { get; set; }
	}
}
