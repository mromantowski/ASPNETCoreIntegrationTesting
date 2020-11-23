using MediatR;
using ProductionService.Model.Cuts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductionService.Service
{
    public class GetCutTaskDetails
    {
        public class Request : IRequest<Response>
        {
            public string TaskId { get; set; }
        }

        public class CutTaskDetails
        {
            public string ItemCode { get; set; }
            public int Count { get; set; }
        }

        public class Response : ResponseBase
        {
            public List<CutTaskDetails> Details { get; set; } = new List<CutTaskDetails>();
        }

        public class Hander : IRequestHandler<Request, Response>
        {
            private readonly ICutTaskReader cutTaskReader;

            public Hander(ICutTaskReader cutTaskReader)
            {
                this.cutTaskReader = cutTaskReader;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    var cutTask = await cutTaskReader.Read(request.TaskId);
                    return new Response 
                    { 
                        Details = cutTask
                            .Details
                            .Select(d => 
                                new CutTaskDetails
                                {
                                    ItemCode = d.ItemCode,
                                    Count = d.Count
                                })
                            .ToList() 
                    };
                }
                catch(Exception e)
                {
                    return new Response { ErrorMessage = $"Nie udało się odczytać danych zlecenia {(e.Message)}" };
                }
            }
        }
    }
}
