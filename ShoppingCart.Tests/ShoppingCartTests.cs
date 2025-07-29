using System.Net;
using ShoppingCart.Price;
using Xunit;

namespace ShoppingCart.Tests
{
	public class ShoppingCartTests
	{
		private PriceClient CreateFakeClient()
		{
			var handler = new FakeHttpMessageHandler(request =>
			{
				var product = request.RequestUri!.Segments.Last().Replace(".json", "");
				decimal price = product switch
				{
					"cheerios" => 8.43m,
					"cornflakes" => 2.52m,
					"frosties" => 4.99m,
					"shreddies" => 4.68m,
					"weetabix" => 9.98m,
					_ => throw new Exception("Invalid product")
				};

				var json = $"{{\"price\":{price}}}";
				return new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(json)
				};
			});
			var httpClient = new HttpClient(handler);
			return new PriceClient(httpClient);
		}

		[Fact]
		public async Task AddProduct_Should_Add_New_Item()
		{
			var priceClient = CreateFakeClient();
			var cart = new Cart.ShoppingCart(priceClient);

			await cart.AddProductAsync("cornflakes", 2);

			var item = cart.Items.First();
			Assert.Equal("cornflakes", item.ProductName);
			Assert.Equal(2, item.Quantity);
			Assert.Equal(2.52m, item.UnitPrice);
		}

		[Fact]
		public async Task AddProduct_Should_Aggregate_Quantity()
		{
			var priceClient = CreateFakeClient();
			var cart = new Cart.ShoppingCart(priceClient);

			await cart.AddProductAsync("cornflakes", 2);
			await cart.AddProductAsync("cornflakes", 2);


			var item = cart.Items.First();
			Assert.Equal(4, item.Quantity);
		}

		[Fact]
		public async Task AddProduct_Should_Calculate_Total_SubTotal_Tax()
		{
			var priceClient = CreateFakeClient();
			var cart = new Cart.ShoppingCart(priceClient);

			await cart.AddProductAsync("cornflakes", 2);
			await cart.AddProductAsync("weetabix", 1);


			Assert.Equal(15.02m, cart.SubTotal);
			Assert.Equal(1.88m, cart.Tax);
			Assert.Equal(16.9m, cart.Total);
		}

        [Fact]
        public async Task Subtotal_Rounds_To_Two_Decimal_Places()
        {
            var priceClient = CreateFakeClient();
            var cart = new Cart.ShoppingCart(priceClient);

            await cart.AddProductAsync("shreddies",5); 

            Assert.Equal(23.4m, cart.SubTotal);
        }

        [Fact]
        public async Task Cart_CalculateCorrectTotals()
        {
            var priceClient = CreateFakeClient();
            var cart = new Cart.ShoppingCart(priceClient);

            Assert.Empty(cart.Items);
            Assert.Equal(0, cart.Total);

            await cart.AddProductAsync("cornflakes", 1);
            await cart.AddProductAsync("cornflakes", 1);
            await cart.AddProductAsync("weetabix", 1);

            Assert.Equal(2, cart.Items.First(i => i.ProductName == "cornflakes").Quantity);
            Assert.Equal(1, cart.Items.First(i => i.ProductName == "weetabix").Quantity);

            Assert.Equal(15.02m, cart.SubTotal);
            Assert.Equal(1.88m, cart.Tax);
            Assert.Equal(16.90m, cart.Total);
        }

		//[Fact]
		//public async Task Add_Zero_Quantity_Should_Throw_Exception()
		//{
  //          var priceClient = CreateFakeClient();
  //          var cart = new Cart.ShoppingCart(priceClient);

		//	var exception = await Assert.ThrowsAsync<Exception>(() => cart.AddProductAsync("cornflakes", -1));
		//	await cart.AddProductAsync("cornflakes", -1);
  //      }
    }
}

