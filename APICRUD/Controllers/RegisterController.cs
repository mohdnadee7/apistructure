using APICRUD.Model;
using APICRUD.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;
        public RegisterController(IRegisterService registerService)
        {
            _registerService= registerService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Post([FromBody] RegisterModel register)
        {
            var result = await _registerService.Add(register);
            return Ok(result);
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _registerService.Get());
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] RegisterModel register, int id)
        {
            return Ok(await _registerService.Update(register,id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _registerService.Delete(id));
        }
    }
}
