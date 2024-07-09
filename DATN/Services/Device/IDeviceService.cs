using DATN.CustomModels;
using DATN.Models;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Services.DeviceService
{
    public interface IDeviceService
    {
        IActionResult GetAll([FromQuery] PaginationFilter filter, int warehouseID);
        IActionResult GetByName([FromQuery] PaginationFilter filter, string descr, int warehouseID);
        IActionResult Create(CreateDevice device);
        IActionResult Delete(int id);
        IActionResult Update(CreateDevice device);
        IActionResult GetAll([FromQuery] PaginationFilter filter);
        IActionResult GetByName([FromQuery] PaginationFilter filter, string name);
        IActionResult GetByID(int id, int warehouseID);
    }
}
