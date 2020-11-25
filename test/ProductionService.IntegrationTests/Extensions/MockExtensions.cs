using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProductionService.IntegrationTests.Extensions
{
    public static class MockExtensions
    {
        public static async Task<bool> WaitUntilInvoked(this Mock mock, int times, TimeSpan timeout)
        {
            var waitUntil = DateTime.Now.Add(timeout);

            while (mock.Invocations.Count < times && DateTime.Now < waitUntil)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }

            return mock.Invocations.Count == times;
        }
    }
}
