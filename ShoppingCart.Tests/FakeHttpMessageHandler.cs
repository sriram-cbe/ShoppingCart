using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCart.Tests
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responseFunc;

        public FakeHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFunc)
        {
            _responseFunc = responseFunc;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_responseFunc(request));
        }
    }
}

