using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductionService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
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
