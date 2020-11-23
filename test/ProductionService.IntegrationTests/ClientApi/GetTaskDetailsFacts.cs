using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProductionService.IntegrationTests.ClientApi
{
    [Trait("Category", "Integration")]
    public class GetTaskDetailsFacts : IntegrationTestsBase
    {
        [Theory]
        [InlineData("2234")]
        [InlineData("2235")]
        public async Task GetTaskDetails_ForValidTaskId_ReturnsSuccess(string taskId)
        {
            var response = await ClientApi.GetCutTaskDetails(taskId);
            
            response.IsSuccessful.ShouldBeTrue();
            response.ErrorMessage.ShouldBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1234")]
        [InlineData("12345")]
        public async Task GetTaskDetails_ForInValidTaskId_ReturnsFailure(string taskId)
        {
            var response = await ClientApi.GetCutTaskDetails(taskId);

            response.IsSuccessful.ShouldBeFalse();
        }

        [Fact]
        public async Task GetTaskDetails_ForValidTaskId_ReturnsValidDetails()
        {
            var response = await ClientApi.GetCutTaskDetails("2234");

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
