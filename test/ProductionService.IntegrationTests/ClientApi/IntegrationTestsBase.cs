using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ProductionService.Client;
using System;

namespace ProductionService.IntegrationTests
{
    public class IntegrationTestsBase : IDisposable
    {
        private readonly string TestServerUrl = "http://localhost:3986";
        private readonly IHost host;

        protected IProductionServiceClient ClientApi { get; }

        private bool disposed = false;

        public IntegrationTestsBase()
        {
            host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseUrls(TestServerUrl);
                })
                .Build();

            host.StartAsync();

            ClientApi = new ProductionServiceClient(TestServerUrl);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                host.Dispose();
            }

            disposed = true;
        }
    }
}
