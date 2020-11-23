using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductionService.Client
{
    public interface IProductionServiceClient
    {
        Task<GetActionsResponse> GetActions(string machine, DateTime day);
        Task<GetActionsResponse> GetActions(string machine, DateTime day, CancellationToken cancellationToken);
        Task<GetCutTaskDetailsResponse> GetCutTaskDetails(string taskId);
        Task<GetCutTaskDetailsResponse> GetCutTaskDetails(string taskId, CancellationToken cancellationToken);
        Task<Response> MarkTaskCompleted(string taskId);
        Task<Response> MarkTaskCompleted(string taskId, CancellationToken cancellationToken);
    }
}
