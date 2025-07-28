using System;
namespace ShoppingCart.Tax
{
	public class FlatTaxStrategy : ITaxStrategy
	{
        private readonly decimal _taxRate;
		public FlatTaxStrategy(decimal taxRate)
		{
            _taxRate = taxRate;
		}

        public decimal CalculateTax(decimal subTotal)
        {
            return Math.Round(subTotal * _taxRate, 2, MidpointRounding.AwayFromZero);
        }
    }
}

