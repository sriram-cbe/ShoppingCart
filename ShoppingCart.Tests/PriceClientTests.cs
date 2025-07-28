using System;
using System.Net;
using System.Text;
using System.Text.Json;
using Moq;
using Moq.Protected;
using ShoppingCart.Price;
using Xunit;

namespace ShoppingCart.Tests
{
	public class PriceClientTests
	{
		private HttpClient CreateHttpClientMock(string responseContent, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
		{
			var handlerMock = new Mock<HttpMessageHandler>();
			handlerMock
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()).ReturnsAsync(
				new HttpResponseMessage
				{
					Content = new StringContent(responseContent, Encoding.UTF8, "application/json"),
					StatusCode = httpStatusCode
				});
			return new HttpClient(handlerMock.Object);
		}

		[Fact]
		public async Task GetPriceAsync_Returns_CorrectPrice_WhenValidJson()
		{
			string json = "{\"price\" : 9.98}";
			var httpClient = CreateHttpClientMock(json);
			var priceClient = new PriceClient(httpClient);

			var price = await priceClient.GetPriceAsync("weetabix");
			Assert.Equal(9.98m, price);
		}

		[Fact]
		public async Task GetPriceAsync_Throws_Exception_WhenPriceIsMissing()
		{
			string json = "{\"name\":\"Weetabix\"}";
			var httpClient = CreateHttpClientMock(json);
			var priceClient = new PriceClient(httpClient);

			await Assert.ThrowsAsync<HttpRequestException>(() => priceClient.GetPriceAsync("badjson"));
		}
	}
}

