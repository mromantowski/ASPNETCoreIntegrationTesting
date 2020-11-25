using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductionService.Model;
using ProductionService.Model.Reports;
using System;

namespace ProductionService.IntegrationTests.ClientApi
{
    public abstract class IntegrationTestsWithReports : IntegrationTestsBase
    {
        protected Mock<IReportingServiceClient> ReportingServiceClientMock { get; private set; }

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            base.ConfigureServices(serviceCollection);

            ReportingServiceClientMock = new Mock<IReportingServiceClient>();

            serviceCollection.AddReports(o =>
            {
                o.GenerationInterval = TimeSpan.FromSeconds(1);
            }, ReportingServiceClientMock.Object);
        }
    }
}
