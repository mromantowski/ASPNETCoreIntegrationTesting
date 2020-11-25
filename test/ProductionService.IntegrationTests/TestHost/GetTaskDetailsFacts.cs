using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ProductionService.IntegrationTests.TestHost
{
    [Trait("Category", "Integration")]
    public class GetTaskDetailsFacts : IntegrationTestsWithCuts
    {
        public class Details
        {
            public string ItemCode { get; set; }
            public int Count { get; set; }
        }

        public class Response
        {
            public bool IsSuccessful { get; set; }
            public string ErrorMessage { get; set; }
            public List<Details> Details { get; set; }
        }

        [Theory]
        [InlineData("2234")]
        [InlineData("2235")]
        public async Task GetTaskDetails_ForValidTaskId_ReturnsSuccess(string taskId)
        {
            var response = await GetAsync<Response>($"/api/cuts/{taskId}");

            response.IsSuccessful.ShouldBeTrue();
            response.ErrorMessage.ShouldBeNull();
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("12345")]
        public async Task GetTaskDetails_ForInValidTaskId_ReturnsFailure(string taskId)
        {
            var response = await GetAsync<Response>($"/api/cuts/{taskId}");

            response.IsSuccessful.ShouldBeFalse();
        }

        [Fact]
        public async Task GetTaskDetails_ForValidTaskId_ReturnsValidDetails()
        {
            var response = await GetAsync<Response>($"/api/cuts/2234");

            response.Details.Count.ShouldBe(3);

            response.Details[0].ItemCode.ShouldBe("1230.223.23456");
            response.Details[0].Count.ShouldBe(2);

            response.Details[1].ItemCode.ShouldBe("1730.223.66781");
            response.Details[1].Count.ShouldBe(4);

            response.Details[2].ItemCode.ShouldBe("1780.223.77432");
            response.Details[2].Count.ShouldBe(3);
        }
    }
}
