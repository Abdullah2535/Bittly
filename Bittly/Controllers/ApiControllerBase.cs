using Bittly.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace Bittly.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult HandleFailure(Error error)
        {
            return error.Type switch
            {
                ErrorType.NotFound => NotFound(new { error.Code, error.Description }),
                ErrorType.Conflict => Conflict(new { error.Code, error.Description }),
                ErrorType.Validation => BadRequest(new { error.Code, error.Description }),
                ErrorType.Unauthorized => Unauthorized(new { error.Code, error.Description }),
                _ => StatusCode(500, new { error.Code, error.Description })
            };
        }
    }
}