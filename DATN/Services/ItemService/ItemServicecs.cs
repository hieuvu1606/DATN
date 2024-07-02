using DATN.CustomModels;
using DATN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace DATN.Services.ItemService
{
    public class ItemServicecs : IITemService
    {
        private readonly DeviceContext _db;

        public ItemServicecs(DeviceContext db)
        {
            _db = db;
        }

        public IActionResult GetAll()
        {
            var lst = _db.Items.ToList();
            return new OkObjectResult(lst);
        }

        public IActionResult GetByDeviceID(int deviceId)
        {
            var lst = _db.Items.Where(p => p.DeviceId == deviceId).ToList();
            return new OkObjectResult(lst);
        }

        public IActionResult GetByID(int itemID)
        {
            var item = _db.Items.Where(p => p.ItemId == itemID);
            return new OkObjectResult(item);
        }

        public IActionResult Update(Item item)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var find = _db.Items.FirstOrDefault(p => p.ItemId == item.ItemId);
                    if (find == null)
                    {
                        return new BadRequestObjectResult(new { success = false, error = "Can't Found Item" });
                    }

                    Update_Item(ref find, item, false);

                    _db.SaveChanges();
                    transaction.Commit();

                    return new OkObjectResult(new { succes = true, message = "Update Item Success" });
                }catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { success = false, error = "Can't Update Item" + ex.Message});
                }
            }
        }

        public IActionResult Delete(int itemID)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var find = _db.Items.FirstOrDefault(p => p.ItemId == itemID);
                    if (find == null)
                    {
                        return new BadRequestObjectResult(new { success = false, error = "Can't Found Item" });
                    }

                    _db.Items.Remove(find);

                    _db.SaveChanges();
                    transaction.Commit();

                    return new OkObjectResult(new { succes = true, message = "Update Item Success" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { success = false, error = "Can't Update Item" + ex.Message });
                }
            }
        }

        public IActionResult Create(CreateItem item)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var newItem = new Item();
                    newItem.DeviceId = item.DeviceId;
                    newItem.WarrantyPeriod = item.WarrantyPeriod;
                    newItem.MaintenanceTime =  item.MaintenanceTime;

                    if (item.Status != null || item.Status != "")
                        newItem.Status = item.Status;
                    else
                        newItem.Status = "";


                    if (item.LastMaintenance.HasValue)
                        newItem.LastMaintenance = new DateOnly(item.LastMaintenance.Value.Year, item.LastMaintenance.Value.Month, item.LastMaintenance.Value.Day);
                    else
                        newItem.LastMaintenance = new DateOnly(0,0,0);

                    newItem.ImporterId = item.ImporterId;
                    newItem.PosId = item.PosId;
                    newItem.IsStored = true;
                    newItem.ImportDate = DateTime.Now;
                    newItem.Qr = "";

                    _db.Items.Add(newItem);
                    _db.SaveChanges();
                    transaction.Commit();

                    return new OkObjectResult(new { succes = true, message = "Update Item Success" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { success = false, error = "Can't Update Item" + ex.Message });
                }
            }
        }

        private void Update_Item(ref Item newItem, Item item, bool isNew)
        {
            if (isNew)
            {
                newItem.IsStored = true;
                newItem.ImportDate = DateTime.Now;
            }
            else
            {
                newItem.IsStored = item.IsStored;
                newItem.ImportDate = item.ImportDate;
            }
            newItem.DeviceId = item.DeviceId;
            newItem.WarrantyPeriod = item.WarrantyPeriod;
            newItem.MaintenanceTime = item.MaintenanceTime;
            newItem.LastMaintenance = item.LastMaintenance;
            newItem.Status = item.Status;
            newItem.ImporterId = item.ImporterId;
            newItem.PosId = item.PosId;
        }
    }
}
