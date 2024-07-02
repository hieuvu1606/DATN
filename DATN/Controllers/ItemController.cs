using DATN.CustomModels;
using DATN.Models;
using DATN.Services.ItemService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Controllers
{
    [Route("item")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IITemService _itemService;

        public ItemController(IITemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll()
        {
            return _itemService.GetAll();
        }

        [HttpGet]
        [Route("getbyid/{itemID}")]
        public IActionResult GetByID(int itemID)
        {
            return _itemService.GetByID(itemID);
        }

        [HttpGet]
        [Route("getbyDeviceID/{deviceID}")]
        public IActionResult GetByDeviceID(int deviceID)
        {
            return _itemService.GetByDeviceID(deviceID);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update([FromBody]Item item)
        {
            return _itemService.Update(item);
        }

        [HttpDelete]
        [Route("delete/{itemID}")]
        public IActionResult Delete(int itemID)
        {
            return _itemService.Delete(itemID);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody]CreateItem item)
        {
            return _itemService.Create(item);
        }
    }
}
