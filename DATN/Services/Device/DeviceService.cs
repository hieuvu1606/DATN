using DATN.CustomModels;
using DATN.Models;
using DATN.Utils;
using DATN.Utils.Response;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DATN.Services.DeviceService
{
    public class DeviceService : IDeviceService
    {
        private readonly DeviceContext _db;

        public DeviceService(DeviceContext db)
        {
            _db = db;
        }

        public IActionResult GetAll([FromQuery] PaginationFilter filter,int warehouseID)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);

            var lst = (from w in _db.Warehouses
                       join p in _db.Positions on w.WarehouseId equals p.WarehouseId
                       join i in _db.Items on p.PosId equals i.PosId
                       join d in _db.Devices on i.DeviceId equals d.DeviceId into itemsGroup
                       from ig in itemsGroup.DefaultIfEmpty()
                       group new { ig, w, i } by new { w.WarehouseId, w.WarehouseDescr, ig.DeviceId, ig.Descr, ig.ShortDescr, ig.Image } into g
                       select new GetDevice
                       {
                           WarehouseID = g.Key.WarehouseId,
                           WarehouseDescr = g.Key.WarehouseDescr,
                           DeviceID = g.Key.DeviceId,
                           DeviceDescr = g.Key.Descr,
                           DeviceShortDescr = g.Key.ShortDescr,
                           Image = g.Key.Image,
                           CurrentAmount = g.Sum(x => x.ig != null && x.i.IsStored ? 1 : 0),
                           TotalAmount = g.Count(x => x.ig != null)
                       }).Skip((validFilter.page - 1) * validFilter.pageSize)
                        .Take(validFilter.pageSize).ToList();

            if(warehouseID != 0)
            {
                lst = lst.Where(p => p.WarehouseID == warehouseID).ToList();
            }

            var count = lst.Count();

            return new OkObjectResult(new PagedResponse<List<GetDevice>>(lst, validFilter.page, validFilter.pageSize, count, true));
        }

        public IActionResult GetByID(int id)
        {
            var lst = _db.Devices.FirstOrDefault(p => p.DeviceId == id);
            return new OkObjectResult(lst);
        }

        public IActionResult GetByName([FromQuery] PaginationFilter filter, string name, int warehouseID)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);

            var lst = (
                        from w in _db.Warehouses
                        join p in _db.Positions on w.WarehouseId equals p.WarehouseId
                        join i in _db.Items on p.PosId equals i.PosId
                        join d in _db.Devices on i.DeviceId equals d.DeviceId into itemsGroup
                        from ig in itemsGroup.DefaultIfEmpty()
                        group new { ig, w, i } by new { w.WarehouseId, w.WarehouseDescr, ig.DeviceId, ig.Descr, ig.ShortDescr, ig.Image } into g
                        select new GetDevice
                        {
                            WarehouseID = g.Key.WarehouseId,
                            WarehouseDescr = g.Key.WarehouseDescr,
                            DeviceID = g.Key.DeviceId,
                            DeviceDescr = g.Key.Descr,
                            DeviceShortDescr = g.Key.ShortDescr,
                            Image = g.Key.Image,
                            CurrentAmount = g.Sum(x => x.ig != null && x.i.IsStored ? 1 : 0),
                            TotalAmount = g.Count(x => x.ig != null)
                        }
                    )
                    .Where(p => p.DeviceDescr.Contains(name) || p.DeviceShortDescr.Contains(name))
                    .Skip((validFilter.page - 1) * validFilter.pageSize)
                    .Take(validFilter.pageSize)
                    .ToList();
            if (warehouseID != 0)
            {
                lst = lst.Where(p => p.WarehouseID == warehouseID).ToList();
            }

            var count = lst.Count();


            return new OkObjectResult(new PagedResponse<List<GetDevice>>(lst, validFilter.page, validFilter.pageSize, count, true));
        }

        #region Device without warehouse
        public IActionResult GetAll([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);

            var lst = (
                       from d in _db.Devices
                       join i in _db.Items on d.DeviceId equals i.DeviceId into itemsGroup
                       from ig in itemsGroup.DefaultIfEmpty()
                       group new { ig, d } by new { d.DeviceId, d.Descr, d.ShortDescr, d.Image } into g
                       select new GetDevice
                       {
                           DeviceID = g.Key.DeviceId,
                           DeviceDescr = g.Key.Descr,
                           DeviceShortDescr = g.Key.ShortDescr,
                           Image = g.Key.Image,
                           CurrentAmount = g.Sum(x => x.ig != null && x.ig.IsStored ? 1 : 0),
                           TotalAmount = g.Count(x => x.ig != null)
                       })
                       .Skip((validFilter.page - 1) * validFilter.pageSize)
                       .Take(validFilter.pageSize)
                       .ToList();

            var count = _db.Devices.Count();

            return new OkObjectResult(new PagedResponse<List<GetDevice>>(lst, validFilter.page, validFilter.pageSize, count, true));
        }

        public IActionResult GetByName([FromQuery] PaginationFilter filter, string name)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);

            var lst = (
                        from d in _db.Devices
                        join i in _db.Items on d.DeviceId equals i.DeviceId into itemsGroup
                        from ig in itemsGroup.DefaultIfEmpty()
                        group new { ig, d } by new { d.DeviceId, d.Descr, d.ShortDescr, d.Image } into g
                        select new GetDevice
                        {
                            DeviceID = g.Key.DeviceId,
                            DeviceDescr = g.Key.Descr,
                            DeviceShortDescr = g.Key.ShortDescr,
                            Image = g.Key.Image,
                            CurrentAmount = g.Sum(x => x.ig != null && x.ig.IsStored ? 1 : 0),
                            TotalAmount = g.Count(x => x.ig != null)
                        })
                        .Where(p => p.DeviceDescr.Contains(name) || p.DeviceShortDescr.Contains(name))
                        .Skip((validFilter.page - 1) * validFilter.pageSize)
                        .Take(validFilter.pageSize)
                        .ToList();

            var count = lst.Count();


            return new OkObjectResult(new PagedResponse<List<GetDevice>>(lst, validFilter.page, validFilter.pageSize, count, true));
        }

        #endregion

        public IActionResult Create(CreateDevice device)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var checkDescr = _db.Devices.FirstOrDefault(p => p.Descr == device.Descr || p.ShortDescr == device.ShortDescr);
                    if (checkDescr != null)
                    {
                        return new BadRequestObjectResult(new { error = "Invalid Device Name Or Short Name" });
                    }
                    else
                    {
                        var newDevice = new Device();
                        Update_Device(ref newDevice, device);
                        _db.Devices.Add(newDevice);
                        _db.SaveChanges();
                        transaction.Commit();
                        return new OkObjectResult(new { message = "Create Success" });
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { error = ex.Message });
                }
            }              
        }

        public IActionResult Delete(int deviceID)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var device = _db.Devices.FirstOrDefault(p => p.DeviceId == deviceID);
                    if(device == null)
                    {
                        return new BadRequestObjectResult(new {success = false, error = "Can't Found Device"});
                    }
                    _db.Devices.Remove(device);
                    transaction.Commit();
                    return new OkObjectResult(new { success = true , message = "Delete Device And All Reference Data Success"});
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new {error = "Can't Cascade All Reference Data" + ex.Message});
                }
            }
        }

        public IActionResult Update(CreateDevice device)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var find = _db.Devices.FirstOrDefault(p => p.Descr == device.Descr || p.ShortDescr == device.ShortDescr);
                    if(find == null)
                    {
                        return new BadRequestObjectResult(new { error = "Invalid Device Name Or Short Name" });
                    }
                    var change = _db.Devices.FirstOrDefault(p => p.DeviceId == find.DeviceId);
                    if(change == null)
                    {
                        return new BadRequestObjectResult(new { error = "Can't Found Device" });
                    }
                    Update_Device(ref change, device);


                    _db.SaveChanges();
                    transaction.Commit();
                    return new OkObjectResult(new { success = true, message = "Update success" });
                }catch(Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new {success = false, error = "Can't Update Data" + ex.Message });

                }
            }
        }

        private void Update_Device(ref Device newDevice,CreateDevice device)
        {
            newDevice.Descr = device.Descr;
            newDevice.ShortDescr = device.ShortDescr;
            newDevice.DescrFunction = device.DescrFunction;
            newDevice.CategoryId = device.CategoryId;
            newDevice.Image = device.Image;
            newDevice.Pdf = device.Pdf;
        }
    }
}
