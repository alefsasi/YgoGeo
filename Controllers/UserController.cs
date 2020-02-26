using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ygo_geo_api.Models;
using ygo_geo_api.Services;

namespace ygo_geo_api.Controllers {
    [Route ("[controller]")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly UserServices _userService;

        public UserController (UserServices userService) {
            _userService = userService;
        }

        [HttpGet]
        [Route ("Usuarios")]
        [AllowAnonymous]
        public ActionResult GetUsuarios () => Ok (_userService.GetUsuarios ());

    }
}