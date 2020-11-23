﻿using Shouldly;
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
        [InlineData("2234", "CT-2234.txt")]
        [InlineData("2235", "CT-2235.txt")]
        public async Task MarkTaskCompleted_ForValidTaskId_MovesTheFile(string taskId, string filename)
        {
            await ClientApi.MarkTaskCompleted(taskId);

            File.Exists(Path.Combine(SourceCutsPath, filename)).ShouldBeFalse();
            File.Exists(Path.Combine(CompletedCutsPath, filename)).ShouldBeTrue();
        }
    }
}