using DATN.CustomModels;
using DATN.Models;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Services.ItemService
{
    public interface IITemService
    {
        IActionResult GetAll();
        IActionResult GetByDeviceID(int deviceId);
        IActionResult GetByID(int itemID);
        IActionResult Update(Item item);
        IActionResult Delete(int id);
        IActionResult Create(CreateItem item);
        IActionResult GetTree(int warehouseID, int registID);
    }
}
