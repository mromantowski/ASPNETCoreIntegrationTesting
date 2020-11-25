using System;
using System.Threading;

namespace ProductionService.Model.Reports
{
    public class ReportGenerator : IReportGenerator, IDisposable
    {
        private readonly IReportingServiceClient reportingServiceClient;
        private readonly TimeSpan generationInterval;
        private readonly AutoResetEvent newReportWaitHandle = new AutoResetEvent(false);
        private readonly AutoResetEvent cancelWaitHandle = new AutoResetEvent(false);
        private readonly Thread worker;

        private bool cancelWorker = false;
        private bool newReportRequired;
        private bool disposed = false;

        public ReportGenerator(IReportingServiceClient reportingServiceClient, TimeSpan generationInterval)
        {
            this.reportingServiceClient = reportingServiceClient;
            this.generationInterval = generationInterval;

            worker = new Thread(DoWork);
            worker.Start();
        }

        public void RequireReportGeneration()
        {
            newReportRequired = true;
            newReportWaitHandle.Set();
        }

        private void DoWork()
        {
            while (!cancelWorker)
            {
                WaitHandle.WaitAny(new[] { newReportWaitHandle, cancelWaitHandle });

                if (newReportRequired && !cancelWorker)
                {
                    reportingServiceClient.CreateReport();
                    cancelWaitHandle.WaitOne(generationInterval);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                cancelWorker = true;
                cancelWaitHandle.Set();

                try
                {
                    worker.Join();
                }
                catch { }
            }

            disposed = true;
        }
    }
}
