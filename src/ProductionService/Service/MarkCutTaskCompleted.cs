using MediatR;
using ProductionService.Model.Cuts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductionService.Service
{
    public class MarkCutTaskCompleted
    {
        public class Request : IRequest<ResponseBase>
        {
            public string TaskId { get; set; }
        }

        public class Hander : IRequestHandler<Request, ResponseBase>
        {
            private readonly ICutTaskManager cutTaskManager;

            public Hander(ICutTaskManager cutTaskManager)
            {
                this.cutTaskManager = cutTaskManager;
            }

            public Task<ResponseBase> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    cutTaskManager.MarkCompleted(request.TaskId);
                    return Task.FromResult(new ResponseBase());
                }
                catch(Exception e)
                {
                    return Task.FromResult(new ResponseBase { ErrorMessage = $"Nie udało się zaznaczyć zlecenia jako zakończone {(e.Message)}" });
                }
            }
        }
    }
}
