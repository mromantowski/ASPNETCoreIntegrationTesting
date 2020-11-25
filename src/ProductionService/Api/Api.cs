using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductionService.Service;
using System.Threading.Tasks;

namespace TestApp1.Controllers
{
    [ApiController]
    public class Api : ControllerBase
    {
        [HttpGet("api/actions")]
        public async Task<GetServiceActions.Response> GetActions([FromServices] IMediator mediator, [FromQuery] GetServiceActions.Request request)
        {
            return await mediator.Send(request);
        }

        [HttpGet("api/cuts/{cutId}")]
        public async Task<GetCutTaskDetails.Response> GetCutDetails([FromServices] IMediator mediator, string cutId)
        {
            return await mediator.Send(new GetCutTaskDetails.Request { TaskId = cutId });
        }

        [HttpPost("api/cuts/completed")]
        public async Task<ResponseBase> MarkCutCompleted([FromServices] IMediator mediator, [FromBody] MarkCutTaskCompleted.Request request)
        {
            return await mediator.Send(request);
        }

        [HttpPost("api/report")]
        public async Task<ResponseBase> RequireRaportGeneration([FromServices] IMediator mediator, [FromBody] RequireRaportGeneration.Request request)
        {
            return await mediator.Send(request);
        }
    }
}
