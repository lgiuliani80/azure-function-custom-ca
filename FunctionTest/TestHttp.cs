using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FunctionTest
{
    public class TestHttp
    {
        private readonly ILogger<TestHttp> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        private static readonly string SERVICE_URL = Environment.GetEnvironmentVariable("SERVICE_URL");

        public TestHttp(ILogger<TestHttp> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [Function("TestHttp")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req
        )
        {
            _logger.LogInformation("TestHttp issued");

            var cli = _httpClientFactory.CreateClient();
            var result = await cli.GetStringAsync(SERVICE_URL);

            return new OkObjectResult("The remote endpoint returned: " + result);
        }
    }
}
