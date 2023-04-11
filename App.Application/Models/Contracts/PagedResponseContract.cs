namespace App.Application.Models.Contracts
{
	public class PagedResponseContract<TData> : TypedResponseContract<TData>
	{
		public int TotalCount { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
	}
}
