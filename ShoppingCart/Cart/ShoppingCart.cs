using System;
using ShoppingCart.Models;
using ShoppingCart.Price;

namespace ShoppingCart.Cart
{
	public class ShoppingCart
	{
		private readonly Dictionary<string, CartItem> _items = new();
        private const decimal TaxRate = 0.125m;

        private readonly PriceClient _priceClient;

        public ShoppingCart(PriceClient priceClient)
        {
            _priceClient = priceClient;
        }


        private decimal Round(decimal value)
        {
            return Math.Round(value,2, MidpointRounding.AwayFromZero);
        }

        public async Task AddProductAsync(string productName, int quantity)
        {
            if (_items.ContainsKey(productName))
                _items[productName].AddQuantity(quantity);
            else
            {
                var price = await _priceClient.GetPriceAsync(productName);
                _items[productName] = new CartItem(productName, quantity, price);
            }
        }
        public IReadOnlyCollection<CartItem> Items => _items.Values;

        public decimal SubTotal => Round(_items.Values.Sum(i => i.TotalPrice));
        public decimal Tax => Round(SubTotal * TaxRate);
        public decimal Total => Round(SubTotal + Tax);
    }
}

