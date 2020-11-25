using Moq;
using ProductionService.IntegrationTests.Extensions;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProductionService.IntegrationTests.TestHost
{
    [Trait("Category", "Integration")]
    public class RequireReportGenerationFacts : IntegrationTestsWithReports
    {
        public class Response
        {
            public bool IsSuccessful { get; set; }
            public string ErrorMessage { get; set; }
        }

        [Fact]
        public async Task RequireReportGeneration_WhenInvoked_ReturnsSuccess()
        {
            var response = await PostAsync<Response>("/api/report", new { });

            response.IsSuccessful.ShouldBeTrue();
            response.ErrorMessage.ShouldBeNull();
        }

        [Fact]
        public async Task RequireReportGeneration_WhenInvoked_InvokesReportGeneration()
        {
            await PostAsync<Response>("/api/report", new { });
            await ReportingServiceClientMock.WaitUntilInvoked(1, TimeSpan.FromSeconds(3));

            ReportingServiceClientMock.Verify(m => m.CreateReport(), Times.Once());
        }

        [Fact]
        public async Task RequireReportGeneration_WhenInvokedMultipleTimes_InvokesReportGenerationOnce()
        {
            // Arrange
            await PostAsync<Response>("/api/report", new { });
            await ReportingServiceClientMock.WaitUntilInvoked(1, TimeSpan.FromSeconds(3));
            ReportingServiceClientMock.Invocations.Clear();

            // Act
            await PostAsync<Response>("/api/report", new { });
            await PostAsync<Response>("/api/report", new { });
            await PostAsync<Response>("/api/report", new { });
            await PostAsync<Response>("/api/report", new { });

            // Assert
            await ReportingServiceClientMock.WaitUntilInvoked(3, TimeSpan.FromSeconds(3));
            ReportingServiceClientMock.Verify(m => m.CreateReport(), Times.Once());
        }
    }
}
