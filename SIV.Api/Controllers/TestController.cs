using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIV.Api.Authorization;

namespace SIV.Api.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    
    public class TestController : ControllerBase
    {
        [Authorize(Policy = "role")]
        [HttpGet("A")]
        public IActionResult A()
        {
            return Ok("A");
        }
        [Authorize(Policy = "role")]
        [HttpGet("B")]
        public IActionResult B()
        {
            return Ok("B");
        }

        [HttpGet("C")]
        public IActionResult C()
        {
            return Ok("C");
        }

        [HttpGet("D")]
        public IActionResult D()
        {
            return Ok("D");
        }
    }
}
