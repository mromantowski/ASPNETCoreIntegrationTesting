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
        public async Task<GetServiceActions.Response> Get([FromServices] IMediator mediator, [FromQuery] GetServiceActions.Request request)
        {
            return await mediator.Send(request);
        }
    }
}
