using DATN.Models;
using DATN.Services.Warehouse;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Controllers
{
    [Route("warehouse")]
    [ApiController]
    public class WareHouseController : Controller
    {
        private IWarehouseService _warehouseService;

        public WareHouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody]Warehouse warehouse)
        {
            return _warehouseService.Create(warehouse);
        }
        [HttpDelete]
        [Route("delete/{warehouseID}")]
        public IActionResult Delete(int warehouseID)
        {
            return _warehouseService.Delete(warehouseID);
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll([FromQuery] PaginationFilter filter)
        {
            return _warehouseService.GetAll(filter);
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public IActionResult GetByID(int id)
        {
            return _warehouseService.GetByID(id);
        }

        [HttpGet]
        [Route("getbyname/{name}")]
        public IActionResult GetByName(string name)
        {
            return _warehouseService.GetByName(name);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update([FromBody] Warehouse warehouse)
        {
            return _warehouseService.Update(warehouse);
        }

        [HttpPost]
        [Route("getPos/{warehouseID}")]
        public IActionResult GetPos([FromQuery] PaginationFilter filter,int warehouseID)
        {
            return _warehouseService.GetPos(filter, warehouseID);
        }

        [HttpPost]
        [Route("createPos/{warehouseID}&{descr}")]
        public IActionResult CreatePos(int warehouseID, string descr)
        {
            return _warehouseService.CreatePos(warehouseID, descr);
        }

        [HttpPost]
        [Route("updatePos")]
        public IActionResult UpdatePos([FromBody] Position pos)
        {
            return _warehouseService.UpdatePos(pos);
        }

        [HttpDelete]
        [Route("deletePos")]
        public IActionResult DeletePos([FromBody] Position pos)
        {
            return _warehouseService.DeletePos(pos);
        }
    }
}
