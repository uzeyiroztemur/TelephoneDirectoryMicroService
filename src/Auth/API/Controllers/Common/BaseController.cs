using Core.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IActionResult ActionResultInstance<T>(IDataResult<T> result) where T : class
        {
            if (result == null)
                return BadRequest();

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        public IActionResult ActionResultInstance<T>(Core.Entities.DTOs.IResult result) where T : class
        {
            if (result == null)
                return BadRequest();

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
