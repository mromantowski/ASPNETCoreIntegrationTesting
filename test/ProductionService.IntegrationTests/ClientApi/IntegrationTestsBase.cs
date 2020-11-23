using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ProductionService.Client;
using ProductionService.Model;
using System;
using System.IO;
using System.Reflection;

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
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var testCutsPath = Path.Combine(basePath, "Files", "Cuts");

            host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<StartupBase>()
                        .ConfigureServices(s => s.AddCuts(testCutsPath))
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
