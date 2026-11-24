using System.Net;
using System.Text;

namespace WeatherApp.Tests
{
    // A fake "internet" for tests. Instead of calling the real weather API,
    // the HttpClient gets the status code and JSON that I choose.
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _json;

        public string? LastUrl { get; private set; }

        public FakeHttpMessageHandler(HttpStatusCode statusCode, string json)
        {
            _statusCode = statusCode;
            _json = json;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastUrl = request.RequestUri?.ToString();

            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_json, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }
    }
}
