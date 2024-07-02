using DATN.Services.RoleService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Controllers
{
    [Route("role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("get")]
        public IActionResult Get()
        {
            return _roleService.Get();
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update(FormCollection data)
        {
            return _roleService.Update(data);
        }
    }
}
