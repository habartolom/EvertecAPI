namespace App.Application.Models.Contracts
{
	public class TypedResponseContract<TData> : ResponseContract
	{
		public TData? Data { get; set; }
	}
}
