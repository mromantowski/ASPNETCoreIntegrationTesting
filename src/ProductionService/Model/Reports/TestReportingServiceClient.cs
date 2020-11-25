using System;

namespace ProductionService.Model.Reports
{
    public class TestReportingServiceClient : IReportingServiceClient
    {
        public void CreateReport()
        {
            Console.WriteLine("New report generated");
        }
    }
}
