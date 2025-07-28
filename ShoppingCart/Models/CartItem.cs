using System;
namespace ShoppingCart.Models
{
	public class CartItem
	{
        public CartItem(string productName, int quantity, decimal unitPrice)
        {
            ProductName = productName.ToLowerInvariant() ;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public string ProductName { get; set; }
        public int Quantity { get;  set; }
        public decimal UnitPrice { get; set; }

        public void AddQuantity(int quantity) => Quantity += quantity;

        public decimal TotalPrice => Quantity * UnitPrice;
	}
}

