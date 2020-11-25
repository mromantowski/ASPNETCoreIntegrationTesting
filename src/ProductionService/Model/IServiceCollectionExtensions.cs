using Microsoft.Extensions.DependencyInjection;
using ProductionService.Model.Cuts;
using ProductionService.Model.Reports;
using ProductionService.Model.ServiceActions;
using System;

namespace ProductionService.Model
{
    public static class IServiceCollectionExtensions
    {
        public static void AddServiceActions(this IServiceCollection services)
        {
            services.AddAllImplementing<IServiceActionProvider>(typeof(IServiceActionProvider));
        }

        public static void AddCuts(this IServiceCollection services, Action<CutsOptions> adjustOptions)
        {
            var options = new CutsOptions();
            adjustOptions(options);

            services.AddTransient<ICutTaskReader, CutTaskReader>((s) => new CutTaskReader(options.SourceCutsPath));
            services.AddTransient<ICutTaskManager, CutTaskManager>((s) => new CutTaskManager(options.SourceCutsPath, options.CompletedCutsPath));
        }

        public static void AddReports(this IServiceCollection services, Action<ReportsOptions> adjustOptions, IReportingServiceClient client = null)
        {
            var options = new ReportsOptions();
            adjustOptions(options);

            if (client == null)
            {
                services.AddSingleton<IReportingServiceClient, TestReportingServiceClient>();
            } 
            else
            {
                services.AddSingleton<IReportingServiceClient>(client);
            }
            services.AddSingleton<IReportGenerator, ReportGenerator>((s) => new ReportGenerator(s.GetRequiredService<IReportingServiceClient>(), options.GenerationInterval));
        }
    }
}
