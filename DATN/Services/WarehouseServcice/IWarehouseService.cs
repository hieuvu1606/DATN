using DATN.Models;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;
using WareHouse = DATN.Models.Warehouse;
namespace DATN.Services.Warehouse
{
    public interface IWarehouseService
    {
        IActionResult Create(WareHouse warehouse);
        IActionResult Delete(int warehouseID);
        IActionResult GetAll([FromQuery] PaginationFilter filter);
        IActionResult GetByID(int id);
        IActionResult GetByName(string name);
        IActionResult Update(WareHouse data);
        IActionResult GetPos([FromQuery] PaginationFilter filter, int warehouseID);
        IActionResult CreatePos(int warehouseID, string descr);
        IActionResult UpdatePos(Position pos);
        IActionResult DeletePos(Position pos);
    }
}
