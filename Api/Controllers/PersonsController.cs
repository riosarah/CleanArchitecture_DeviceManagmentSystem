using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController(IMediator mediator) : ControllerBase
    {
       
    }
}
