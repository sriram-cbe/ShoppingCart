using System;
namespace ShoppingCart.Tax
{
	public interface ITaxStrategy
	{
		public decimal CalculateTax(decimal subTotal);
	}
}

