using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ShoppingCart.Models;

namespace ShoppingCart.Price
{
	public class PriceClient
	{
		private readonly HttpClient _httpClient;

		private const string BaseUrl = "https://equalexperts.github.io//backend-take-home-test-data/";

        public PriceClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<decimal> GetPriceAsync(string productName)
		{
			var requesUurl = $"{BaseUrl}{productName.ToLowerInvariant()}.json";
			var response = await _httpClient.GetStringAsync(requesUurl);


			var product = JsonSerializer.Deserialize<ProductPrice>(response);
			return product?.price ?? throw new Exception("Invalid product data");
		}
	}
}

