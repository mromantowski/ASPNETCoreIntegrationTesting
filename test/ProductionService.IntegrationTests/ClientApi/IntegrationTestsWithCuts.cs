using Microsoft.Extensions.DependencyInjection;
using ProductionService.Model;
using System;
using System.IO;
using System.Reflection;

namespace ProductionService.IntegrationTests.ClientApi
{
    public abstract class IntegrationTestsWithCuts : IntegrationTestsBase
    {
        private string basePath;
        protected string SourceCutsPath { get; private set; }
        protected string CompletedCutsPath { get; private set; }

        private bool disposed = false;

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            base.ConfigureServices(serviceCollection);

            basePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            SourceCutsPath = Path.Combine(basePath, "Source");
            CompletedCutsPath = Path.Combine(basePath, "Completed");

            InitTestDirectories();

            serviceCollection.AddCuts(o =>
            {
                o.SourceCutsPath = SourceCutsPath;
                o.CompletedCutsPath = CompletedCutsPath;
            });
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                Directory.Delete(basePath, true);
            }

            disposed = true;
        }
    }
}
