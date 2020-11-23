using MediatR;
using ProductionService.Model.ServiceActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductionService.Service
{
    public class GetServiceActions
    {
        public class Request : IRequest<Response>
        {
            public DateTime Day { get; set; }
            public string Machine { get; set; }
        }

        public class Response : ResponseBase
        {
            public List<string> Actions { get; set; } = new List<string>();
        }

        public class Hander : IRequestHandler<Request, Response>
        {
            private readonly IEnumerable<IServiceActionProvider> serviceActionProviders;

            public Hander(IEnumerable<IServiceActionProvider> serviceActionProviders)
            {
                this.serviceActionProviders = serviceActionProviders;
            }

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (Enum.TryParse<Machine>(request.Machine, true, out Machine machine))
                {
                    var result = new Response
                    {
                        Actions = serviceActionProviders
                            .SelectMany(a => a.GetActions(machine, request.Day.ToLocalTime()))
                            .ToList()
                    };
                    return Task.FromResult(result);
                }
                else
                {
                    return Task.FromResult(new Response { ErrorMessage = $"Nie ma takiej maszyny: {request.Machine}" });
                }
            }
        }
    }
}
