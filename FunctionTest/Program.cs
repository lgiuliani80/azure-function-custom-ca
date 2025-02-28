using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;
using FunctionTest;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddHttpClient();
        services.AddHttpClient("customValidation").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => 
            {
                var caThumbprint = Environment.GetEnvironmentVariable("CA_THUMBPRINT");

                if (cert is null || chain is null)
                    return false;

                var caCert = caThumbprint is null ? null : AppServiceCertificate.GetCertificate(caThumbprint);

                if (caCert is not null)
                {
                    chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
                    chain.ChainPolicy.CustomTrustStore.Add(caCert);
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                }

                return chain.Build(cert);
            }
        });
    })
    .Build();

host.Run();
