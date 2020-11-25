using MediatR;
using ProductionService.Model.Reports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductionService.Service
{
    public class RequireRaportGeneration
    {
        public class Request : IRequest<ResponseBase>
        {
        }

        public class Hander : IRequestHandler<Request, ResponseBase>
        {
            private readonly IReportGenerator reportGenerator;

            public Hander(IReportGenerator reportGenerator)
            {
                this.reportGenerator = reportGenerator;
            }

            public Task<ResponseBase> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    reportGenerator.RequireReportGeneration();
                    return Task.FromResult(new ResponseBase());
                }
                catch(Exception e)
                {
                    return Task.FromResult(new ResponseBase { ErrorMessage = $"Nie udało się zlecić wykonania raportu {(e.Message)}" });
                }
            }
        }
    }
}
