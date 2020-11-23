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

        private readonly string basePath;
        protected string SourceCutsPath { get; }
        protected string CompletedCutsPath { get; }

        private bool disposed = false;

        public IntegrationTestsBase()
        {
            basePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            SourceCutsPath = Path.Combine(basePath, "Source");
            CompletedCutsPath = Path.Combine(basePath, "Completed");

            InitTestDirectories();

             host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<StartupBase>()
                        .ConfigureServices(s => s.AddCuts(SourceCutsPath, CompletedCutsPath))
                        .UseUrls(TestServerUrl);
                })
                .Build();

            host.StartAsync();

            ClientApi = new ProductionServiceClient(TestServerUrl);
        }

        private void InitTestDirectories()
        {
            var assemblyBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var orginalCutsPath = Path.Combine(assemblyBasePath, "Files", "Cuts");

            Directory.CreateDirectory(SourceCutsPath);

            foreach (var file in Directory.GetFiles(orginalCutsPath))
            {
                File.Copy(file, Path.Combine(SourceCutsPath, Path.GetFileName(file)));
            }

            Directory.CreateDirectory(CompletedCutsPath);
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
                Directory.Delete(basePath, true);
            }

            disposed = true;
        }
    }
}
