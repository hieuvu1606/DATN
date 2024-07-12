using DATN.CustomModels;
using DATN.Models;
using DATN.Services.DeviceService;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Controllers
{
    [Route("device")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        public DeviceController(IDeviceService dbdeviceService)
        {
            _deviceService = dbdeviceService;
        }

        [HttpGet]
        [Route("getall/{warehouseID}")]
        public IActionResult GetAll([FromQuery] PaginationFilter filter, int warehouseID)
        {
            return _deviceService.GetAll(filter, warehouseID);
        }

        [HttpGet]
        [Route("getbyid/{deviceID}&{warehouseID}")]
        public IActionResult GetByID(int deviceID, int warehouseID)
        {
            return _deviceService.GetByID(deviceID, warehouseID);
        }

        [HttpGet]
        [Route("getbyname/{name}&{warehouseID}")]
        public IActionResult GetByName([FromQuery] PaginationFilter filter, string name, int warehouseID)
        {
            return _deviceService.GetByName(filter, name, warehouseID);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] CreateDevice device)
        {
            return _deviceService.Create(device);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            return _deviceService.Delete(id);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update([FromBody] CreateDevice device)
        {
            return _deviceService.Update(device);
        }


        [HttpGet]
        [Route("getalldevice")]
        public IActionResult GetAllDevice([FromQuery] PaginationFilter filter)
        {
            return _deviceService.GetAll(filter);
        }

        [HttpGet]
        [Route("findbyname/{name}")]
        public IActionResult FindByName([FromQuery] PaginationFilter filter, string name)
        {
            return _deviceService.GetByName(filter, name);
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public IActionResult GetByID(int id)
        {
            return _deviceService.GetByID(id);
        }
    }
}
