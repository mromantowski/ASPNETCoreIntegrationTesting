using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductionService.Client
{
    public interface IProductionServiceClient
    {
        Task<GetActionsResponse> GetActions(string machine, DateTime day);
        Task<GetActionsResponse> GetActions(string machine, DateTime day, CancellationToken cancellationToken);
    }
}
