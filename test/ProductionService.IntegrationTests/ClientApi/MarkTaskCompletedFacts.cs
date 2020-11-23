using Shouldly;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ProductionService.IntegrationTests.ClientApi
{
    [Trait("Category", "Integration")]
    public class MarkTaskCompletedFacts : IntegrationTestsBase
    {
        [Theory]
        [InlineData("2234")]
        [InlineData("2235")]
        public async Task MarkTaskCompleted_ForValidTaskId_ReturnsSuccess(string taskId)
        {
            var response = await ClientApi.MarkTaskCompleted(taskId);
            
            response.IsSuccessful.ShouldBeTrue();
            response.ErrorMessage.ShouldBeNull();
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("12345")]
        public async Task MarkTaskCompleted_ForInValidTaskId_ReturnsFailure(string taskId)
        {
            var response = await ClientApi.MarkTaskCompleted(taskId);

            response.IsSuccessful.ShouldBeFalse();
            response.ErrorMessage.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("2234")]
        [InlineData("2235")]
        public async Task MarkTaskCompleted_ForValidTaskId_MovesTheFile(string taskId)
        {
            await ClientApi.MarkTaskCompleted(taskId);

            File.Exists(Path.Combine(SourceCutsPath, $"CT-{taskId}.txt")).ShouldBeFalse();
            File.Exists(Path.Combine(CompletedCutsPath, $"CT-{taskId}.txt")).ShouldBeTrue();
        }
    }
}
