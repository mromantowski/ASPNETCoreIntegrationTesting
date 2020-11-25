using Moq;
using ProductionService.IntegrationTests.Extensions;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProductionService.IntegrationTests.ClientApi
{
    [Trait("Category", "Integration")]
    public class RequireReportGenerationFacts : IntegrationTestsWithReports
    {
        [Fact]
        public async Task RequireReportGeneration_WhenInvoked_ReturnsSuccess()
        {
            var response = await ClientApi.RequireReportGeneration();
            
            response.IsSuccessful.ShouldBeTrue();
            response.ErrorMessage.ShouldBeNull();
        }

        [Fact]
        public async Task RequireReportGeneration_WhenInvoked_InvokesReportGeneration()
        {
            await ClientApi.RequireReportGeneration();
            await ReportingServiceClientMock.WaitUntilInvoked(1, TimeSpan.FromSeconds(3));

            ReportingServiceClientMock.Verify(m => m.CreateReport(), Times.Once());
        }

        [Fact]
        public async Task RequireReportGeneration_WhenInvokedMultipleTimes_InvokesReportGenerationOnce()
        {
            // Arrange
            await ClientApi.RequireReportGeneration();
            await ReportingServiceClientMock.WaitUntilInvoked(1, TimeSpan.FromSeconds(3));
            ReportingServiceClientMock.Invocations.Clear();

            // Act
            await ClientApi.RequireReportGeneration();
            await ClientApi.RequireReportGeneration();
            await ClientApi.RequireReportGeneration();
            await ClientApi.RequireReportGeneration();

            // Assert
            await ReportingServiceClientMock.WaitUntilInvoked(3, TimeSpan.FromSeconds(3));
            ReportingServiceClientMock.Verify(m => m.CreateReport(), Times.Once());
        }
    }
}
