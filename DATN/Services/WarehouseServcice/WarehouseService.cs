using DATN.Models;
using DATN.Utils;
using DATN.Utils.Response;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WareHouse = DATN.Models.Warehouse;

namespace DATN.Services.Warehouse
{
    public class WarehouseService : IWarehouseService
    {
        private readonly DeviceContext _db;
        public WarehouseService(DeviceContext db)
        {
            _db = db;
        }

        #region Warehoouse
        public IActionResult GetAll([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);
            var lst = _db.Warehouses.Skip((validFilter.page - 1) * validFilter.pageSize)
                                    .Take(validFilter.pageSize).ToList();
            var count = _db.Warehouses.Count();

            return new OkObjectResult(new PagedResponse<List<WareHouse>>(lst, validFilter.page, validFilter.pageSize, count, true));
        }

        public IActionResult GetByID(int id)
        {
            var warehouse = _db.Warehouses.FirstOrDefault(p => p.WarehouseId == id);
            if(warehouse == null)
            {
                return new BadRequestObjectResult(new {success = false, error = "Can't Found Warehouse"});
            }
            return new OkObjectResult(warehouse);
        }

        public IActionResult GetByName(string name)
        {
            var lst = _db.Warehouses.Where(p => p.WarehouseDescr == name).ToList();
            return new OkObjectResult(lst);
        }

        public IActionResult Update(WareHouse wareHouse)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {

                    var checkAddress = _db.Warehouses.FirstOrDefault(p => p.Address == wareHouse.Address);
                    if (checkAddress != null)
                    {
                        return new BadRequestObjectResult(new {success = false, error = "Address Already Have Warehouse"});
                    }

                    var checkDescr = _db.Warehouses.FirstOrDefault(p => p.WarehouseDescr == wareHouse.WarehouseDescr);
                    if(checkDescr != null)
                    {
                        return new BadRequestObjectResult(new { success = false, error = "Warehouse Name Have Allready Used" });
                    }

                    var warehouse = _db.Warehouses.FirstOrDefault(p => p.WarehouseId == wareHouse.WarehouseId);
                    if (warehouse == null)
                    {
                        return new BadRequestObjectResult(new { success = false, error = "Can't Found Warehouse" });
                    }
                    warehouse.Address = wareHouse.Address;
                    warehouse.WarehouseDescr = wareHouse.WarehouseDescr;
                    _db.SaveChanges();
                    transaction.Commit();
                    return new OkObjectResult(new {succes = true, message = "Update Success"});
                }catch(Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new {success = false, error = ex.Message });
                }
            }
        }

        public IActionResult Delete(int warehouseID)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var warehouse = _db.Warehouses.FirstOrDefault(p => p.WarehouseId == warehouseID);
                    if (warehouse == null)
                    {
                        return new BadRequestObjectResult(new { success = false, error = "Can't Found Warehouse" });
                    }
                    _db.Remove(warehouse);
                    _db.SaveChanges();
                    transaction.Commit();
                    return new OkObjectResult(new { success = true, message = "Warehouse And All Reference Data Delete Success" });
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new {success = false, error = "" + ex.Message});
                }
            }
        }
        
        public IActionResult Create(WareHouse wareHouse)
        {
            using( var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    
                    var checkAddress = _db.Warehouses.FirstOrDefault(p => p.Address == wareHouse.Address);
                    if (checkAddress != null)
                    {
                        return new BadRequestObjectResult(new { success = false, error = "Address Already Have Warehouse" });
                    }

                    var checkDescr = _db.Warehouses.FirstOrDefault(p => p.WarehouseDescr == wareHouse.WarehouseDescr);
                    if (checkDescr != null)
                    {
                        return new BadRequestObjectResult(new { success = false, error = "Warehouse Name Have Allready Used" });
                    }

                    var check = _db.Warehouses.FirstOrDefault(p => p.Address == wareHouse.Address && p.WarehouseDescr == wareHouse.WarehouseDescr);
                    if (check != null)
                    {
                        return new BadRequestObjectResult(new { success = false, error = "Address Or WarehouseDescr Is Used" });
                    }
                    var warehouse = new DATN.Models.Warehouse();
                    warehouse.Address = wareHouse.Address;
                    warehouse.WarehouseDescr = wareHouse.WarehouseDescr;

                    _db.Add(warehouse);
                    _db.SaveChanges();

                    transaction.Commit( ); ;
                    return new OkObjectResult(new { success = true, message = "Create New Warehouse Success" });
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { success = false, error = ex.Message});
                }
            }
            
        }

        #endregion

        #region Position
        public IActionResult GetPos([FromQuery] PaginationFilter filter,int warehouseID)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);

            var lst = _db.Positions.Where(p => p.WarehouseId == warehouseID)
                            .Skip((validFilter.page - 1) * validFilter.pageSize)
                            .Take(validFilter.pageSize).ToList();

            var count = _db.Positions.Where(p => p.WarehouseId == warehouseID).Count();

            return new OkObjectResult(new PagedResponse<List<Position>>(lst, validFilter.page, validFilter.pageSize, count, true));
        }

        public IActionResult CreatePos(int warehouseID, string descr)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var find = _db.Positions.FirstOrDefault(p => p.PositionDescr.ToLower() == descr.ToLower());
                    if(find != null)
                    {
                        return new BadRequestObjectResult(new { success = false, error = "Invalid Position Description" });
                    }

                    var newPos = new Position();
                    newPos.PositionDescr = descr;
                    newPos.WarehouseId = warehouseID;

                    _db.Positions.Add(newPos);
                    _db.SaveChanges();
                    transaction.Commit();
                    return new OkObjectResult(new { success = true, message = "Create New Position Success" });
                }catch(Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { success = false, error = "Can't Create New Position" + ex.Message });
                }
            }
        }

        public IActionResult UpdatePos(Position pos)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var find = _db.Positions.FirstOrDefault(p => p.WarehouseId == p.WarehouseId && p.PositionDescr.ToLower() == pos.PositionDescr.ToLower());
                    if (find != null)
                    {
                        return new BadRequestObjectResult(new { success = false, error = "Invalid Position Description" });
                    }

                    var change = _db.Positions.FirstOrDefault(p => p.PosId == pos.PosId && p.WarehouseId == p.WarehouseId);
                    change.PosId = pos.PosId;
                    change.WarehouseId = pos.WarehouseId;

                    _db.SaveChanges();
                    transaction.Commit();
                    return new OkObjectResult(new { success = true, message = "Update Position Success" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { success = false, error = "Can't Update Position" + ex.Message });
                }
            }
        }

        public IActionResult DeletePos(Position pos)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var find = _db.Positions.FirstOrDefault(p => p.WarehouseId == pos.WarehouseId && p.PosId == pos.PosId);
                    if (find == null)
                    {
                        return new BadRequestObjectResult(new { success = false, error = "Cannot Found This Position In Warehouse" });
                    }

                    _db.Remove(find);

                    _db.SaveChanges();
                    transaction.Commit();
                    return new OkObjectResult(new { success = true, message = "Delte Position Success" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { success = false, error = "Can't Delete Position" + ex.Message });
                }
            }
        }
        #endregion
    }
}
