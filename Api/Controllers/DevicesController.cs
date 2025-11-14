using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController(IMediator mediator) : ControllerBase
    {
        
    }
}
