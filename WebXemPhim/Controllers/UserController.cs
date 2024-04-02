﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Services.Interfaces;

namespace WebXemPhim.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpPost("/api/auth/register")]
        public IActionResult Register([FromBody]Requests_Register requests)
        {
            var result = _userServices.Register(requests);
            if(result.Status == 400) { return BadRequest(result); }
            else if (result.Status == 404) { return NotFound(result); }
            return  Ok(result);

        }
        [HttpPost("/api/auth/ConfirmNewAcc")]
        public IActionResult ConfirmNewAcc([FromBody]Requests_ConfirmEmail requests)
        {
            return Ok(_userServices.ConfirmNewAcc(requests));
        }
    }
}
