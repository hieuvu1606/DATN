﻿using Microsoft.AspNetCore.Mvc;
using DATN.Services.UserService;
using DATN.Services.RoleService;
using Microsoft.AspNetCore.Authorization;
using DATN.Models;
using DATN.CustomModels;

namespace DATN.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterUser user)
        {
            return _userService.Register(user);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] Login  login)
        {
           return _userService.Login(login);
        }

        [HttpPost]
        [Route("changepassword")]
        public IActionResult ChangePassword([FromBody]ChangePassword changePassword)
        {
            return _userService.ChangePassword(changePassword);
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult GetAllUsers()
        {
            return _userService.GetBasicInfoUsers();
        }

        [HttpPost]
        [Route("file")]
        public IActionResult File(IFormFile file)
        {
            return new OkObjectResult(file);
        }
        
    }
}
