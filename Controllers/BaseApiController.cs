using Microsoft.AspNetCore.Mvc;
using NOLA_API.Application.Core;
using MediatR;

namespace NOLA_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator => _mediator == null
        ? HttpContext.RequestServices.GetService<IMediator>()!
        : _mediator;

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null)
                return Ok(result.Value);
            if (result.IsSuccess && result.Value == null) return NotFound();
            return BadRequest(result.Error);
        }
    }
}