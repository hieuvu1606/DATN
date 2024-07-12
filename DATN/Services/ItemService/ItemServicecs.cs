using DATN.CustomModels;
using DATN.Models;
using Microsoft.AspNetCore.Mvc;
using DATN.Utils;


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

                    //Minvalue == 0001-01-01T00:00:00Z
                    if (item.LastMaintenance != DateTime.MinValue)
                        newItem.LastMaintenance = DateOnly.FromDateTime(item.LastMaintenance);
                    else
                        newItem.LastMaintenance = DateOnly.MinValue;

                    newItem.ImporterId = item.ImporterId;
                    newItem.PosId = item.PosId;
                    newItem.IsStored = true;
                    newItem.ImportDate = DateTime.Now;
                    newItem.Qr = "";

                    _db.Items.Add(newItem);
                    _db.SaveChanges();

                    newItem.Qr = QRGenerator.QRconvert(newItem.ItemId);
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

        public IActionResult GetTree(int warehouseID, int registID)
        {
            var result = (from w in _db.Warehouses
                          join p in _db.Positions on w.WarehouseId equals p.WarehouseId
                          join i in _db.Items on p.PosId equals i.PosId
                          join d in _db.Devices on i.DeviceId equals d.DeviceId
                          join lr in _db.ListDeviceRegists on d.DeviceId equals lr.DeviceId
                          join dr in _db.DeviceRegistrations on lr.RegistId equals dr.RegistId
                          where w.WarehouseId == warehouseID && lr.RegistId == registID && i.IsStored == true
                          group new { w, d, i } by new { w.WarehouseId, w.WarehouseDescr, d.DeviceId, d.Descr } into g
                          select new
                          {
                              g.Key.WarehouseId,
                              g.Key.WarehouseDescr,
                              g.Key.DeviceId,
                              g.Key.Descr,
                              Items = g.Select(x => x.i.ItemId).ToList()
                          }).ToList();

            var groupedResult = result.GroupBy(r => new { r.WarehouseId, r.WarehouseDescr })
                .Select(grp => new TreeItem
                {
                    WarehouseID = grp.Key.WarehouseId,
                    WarehouseDescr = grp.Key.WarehouseDescr,
                    ListDevice = grp.GroupBy(d => new { d.DeviceId, d.Descr })
                                 .Select(deviceGroup => new DeviceNode
                                 {
                                     DeviceID = deviceGroup.Key.DeviceId,
                                     DeviceDescr = deviceGroup.Key.Descr,
                                     ListItemID = deviceGroup.SelectMany(d => d.Items).Distinct().ToList()
                                 }).ToList()
                }).ToList();

            return new OkObjectResult(groupedResult);
        }


    }
}
