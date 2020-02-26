using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ygo_geo_api.Models;
using ygo_geo_api.Services;

namespace ygo_geo_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthServices _authService;

        public AuthController(AuthServices authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Signup")]
        [AllowAnonymous]
        public IActionResult Cadastrar([FromBody] User user)
        {
            try
            {
               // _authService.Signup(user);
                //return Ok( new {message = "Created"});
                return Ok("cs");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] User user)
        {
            try
            {
                var userLogin = _authService.Authenticate(user);

                if (userLogin == null)
                    return BadRequest(new { message = "Usuário ou senha inválidos!" });

                return Ok(userLogin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}