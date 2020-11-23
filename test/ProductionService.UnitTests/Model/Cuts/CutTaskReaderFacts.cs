using ProductionService.Model.Cuts;
using Shouldly;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace ProductionService.UnitTests.Model.Cuts
{
    [Trait("Category", "Unit")]
    public class CutTaskReaderFacts
    {
        private readonly CutTaskReader reader;

        public CutTaskReaderFacts()
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var testCutsPath = Path.Combine(basePath, "Model", "Cuts");
            
            reader = new CutTaskReader(testCutsPath);
        }

        [Fact]
        public async void Read_WhenValidFile_ReturnsValidCutTask()
        {
            var result = await reader.Read("2234");

            result.ShouldNotBeNull();
            result.TaskId.ShouldBe("2234");
            result.Details.Count.ShouldBe(3);

            result.Details[0].ItemCode.ShouldBe("1230.223.23456");
            result.Details[0].Count.ShouldBe(2);

            result.Details[1].ItemCode.ShouldBe("1730.223.66781");
            result.Details[1].Count.ShouldBe(4);

            result.Details[2].ItemCode.ShouldBe("1780.223.77432");
            result.Details[2].Count.ShouldBe(3);
        }

        [Fact]
        public async void Read_WhenNullTaskId_ThrowsArgumentNullException()
        {
            await Should.ThrowAsync<ArgumentNullException>(() => reader.Read(null));
        }

        [Fact]
        public async void Read_WhenFileIsMissing_ThrowsFileNotFoundException()
        {
            await Should.ThrowAsync<FileNotFoundException>(() => reader.Read("1111"));
        }
    }
}
