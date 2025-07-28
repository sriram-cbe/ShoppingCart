using System;
namespace ShoppingCart.Tax
{
	public class ZeroTaxStrategy : ITaxStrategy
	{
        private readonly decimal _taxRate;
		public ZeroTaxStrategy(decimal taxRate)
		{
            _taxRate = taxRate;
		}

        public decimal CalculateTax(decimal subTotal) => 0m;
         
    }
}

