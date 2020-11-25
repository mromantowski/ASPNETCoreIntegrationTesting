using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductionService.Model;
using ProductionService.Model.Reports;
using System;

namespace ProductionService.IntegrationTests.TestHost
{
    public abstract class IntegrationTestsWithReports : IntegrationTestsBase
    {
        protected Mock<IReportingServiceClient> ReportingServiceClientMock { get; private set; }

        protected override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            base.ConfigureServices(context, services);

            ReportingServiceClientMock = new Mock<IReportingServiceClient>();

            services.AddReports(o =>
            {
                o.GenerationInterval = TimeSpan.FromSeconds(1);
            }, ReportingServiceClientMock.Object);
        }
    }
}
