using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebService.Controllers
{
    [Route("")]
    [ApiController]
    public class SpotifyController : ControllerBase
    {

        [HttpGet("/callback")]
        public void Callback([FromQuery] string? code, [FromQuery] string? state)
        {

        }
    }
}
