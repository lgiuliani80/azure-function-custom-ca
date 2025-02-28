using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.ConstrainedExecution;

namespace FunctionTest
{
    public class TestHttpCustomValidation
    {
        private readonly ILogger<TestHttpCustomValidation> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        private static readonly string SERVICE_URL = Environment.GetEnvironmentVariable("SERVICE_URL")!;

        public TestHttpCustomValidation(
            ILogger<TestHttpCustomValidation> logger,
            IHttpClientFactory httpClientFactory
        )
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [Function("TestHttpCustomValidation")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("TestHttpCustomValidation issued");

            var cli = _httpClientFactory.CreateClient("customValidation");
            var result = await cli.GetStringAsync(SERVICE_URL);

            return new OkObjectResult("The remote endpoint returned: " + result);
        }

        [Function("EnumCertificates")]
        public IActionResult EnumCertificates([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("EnumCertificates issued");

            using X509Store store = new(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var certs = store.Certificates;

            var caThumbprint = Environment.GetEnvironmentVariable("CA_THUMBPRINT");
            var caCert = caThumbprint is null ? null : AppServiceCertificate.GetCertificate(caThumbprint);

            _logger.LogInformation("CA Subject: {subject}", caCert?.Subject);

            return new OkObjectResult(new
            {
                CertStore = certs.Select(x => new { x.Thumbprint, x.Subject }),
                Disk = Directory.GetFiles("/var/ssl/certs/"),
                CACert = new {  caCert?.Thumbprint, caCert?.Subject }
            });
        }
    }
}
