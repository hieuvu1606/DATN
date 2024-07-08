using DATN.CustomModels;
using DATN.Services.RegistDevice;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Controllers
{
    [Route("regist")]
    [ApiController]
    public class RegistDeviceController : Controller
    {
        private readonly IRegistDeviceService _registDeviceService;

        public RegistDeviceController(IRegistDeviceService registDeviceService)
        {
            _registDeviceService = registDeviceService;
        }


        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] RegistForm regist)
        {
            return _registDeviceService.Create(regist);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int registID)
        {
            return _registDeviceService.Delete(registID);
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll(PaginationFilter filter)
        {
            return _registDeviceService.GetAll(filter);
        }

        [HttpGet]
        [Route("getbyid/{registID}")]
        public IActionResult GetById(int registID)
        {
            return _registDeviceService.GetById(registID);
        }

        [HttpGet]
        [Route("getbyuser/{userID}")]
        public IActionResult GetByUser(int userID)
        {
            return _registDeviceService.GetByUser(userID);
        }

        [HttpPost]
        [Route("updateStatus")]
        public IActionResult UpdateStatus([FromBody]UpdateStatusRegist updateStatus)
        {
            return _registDeviceService.UpdateStatus(updateStatus);
        }

        [HttpPost]
        [Route("borrow")]
        public IActionResult Borrow([FromBody]BorrowRegist borrrow)
        {
            return _registDeviceService.Borrow(borrrow);
        }
    }
}
