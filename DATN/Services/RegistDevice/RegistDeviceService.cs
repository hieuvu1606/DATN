using DATN.Models;
using Microsoft.AspNetCore.Mvc;
using DATN.Models;
using DATN.CustomModels;
using Microsoft.Win32;
using DATN.Utils;
using DATN.Utils.Response;
namespace DATN.Services.RegistDevice
{
    public class RegistDeviceService : IRegistDeviceService
    {
        private readonly DeviceContext _db;

        public RegistDeviceService(DeviceContext db)
        {
            _db = db;
        }

        public IActionResult GetAll(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);

            var lst = _db.DeviceRegistrations.Skip((validFilter.page - 1) * validFilter.pageSize)
                 .Take(validFilter.pageSize).ToList();

            var count = lst.Count();

            return new OkObjectResult(new PagedResponse<List<DeviceRegistration>>(lst, validFilter.page, validFilter.pageSize, count, true));
        }

        public IActionResult GetByUser(int userID)
        {
            var lst = _db.DeviceRegistrations.Where(p => p.UserId == userID).ToList();
            return new OkObjectResult(lst);
        }

        public IActionResult GetById(int id)
        {
            var regist = _db.DeviceRegistrations.FirstOrDefault(p => p.RegistId == id);
            return new OkObjectResult(regist);
        }

        public IActionResult Create(RegistForm regist)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var deviceRegist = new DeviceRegistration
                    {
                        UserId = regist.UserID,
                        Proof = null,
                        RegistDate = DateTime.Now,
                        BorrowDate = regist.BorrowDate,
                        ReturnDate = regist.ReturnDate,
                        Status = "Chờ Duyệt",
                        WarehouseId = regist.WarehouseID,
                        ActualBorrowDate = null,
                        ActualReturnDate = null,
                        Reason = ""
                    };

                    _db.DeviceRegistrations.Add(deviceRegist);
                    _db.SaveChanges();

                    foreach (ListDeviceRegist lstDevice in deviceRegist.ListDeviceRegists)
                    {
                        var newList = new ListDeviceRegist
                        {
                            RegistId = deviceRegist.RegistId,
                            DeviceId = lstDevice.DeviceId,
                            BorrowQuantity = lstDevice.BorrowQuantity,
                            ConfirmQuantity = 0
                        };
                        _db.ListDeviceRegists.Add(newList);
                    }
                    _db.SaveChanges();

                    transaction.Commit();
                    return new OkObjectResult(new { success = true, message = "Create New Success" });

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { success = false, error = $"Can't Create New Regist {ex.Message}" });
                }
            }

        }

        public IActionResult UpdateStatus(UpdateStatusRegist updateStatus)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var registForm = _db.DeviceRegistrations.FirstOrDefault(r => r.RegistId == updateStatus.RegistID);

                    if (registForm == null)
                    {
                        return new BadRequestObjectResult(new { message = "RegistForm not found" });
                    }

                    registForm.ManagerId = updateStatus.ManagerID;
                    registForm.Status = updateStatus.Status;
                    registForm.Reason = updateStatus.Reason;

                    _db.SaveChanges();
                    transaction.Commit();


                    return new OkObjectResult(new { success = true, message = "Update Status Success" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { success = false, error = ex.ToString() });
                }
            }
        }

        public IActionResult Delete(int registID)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var registForm = _db.DeviceRegistrations.FirstOrDefault(r => r.RegistId == registID);

                    if (registForm == null)
                    {
                        return new BadRequestObjectResult(new { message = "RegistForm not found" });
                    }

                    _db.DeviceRegistrations.Remove(registForm);

                    _db.SaveChanges();
                    transaction.Commit();

                    return new OkObjectResult(new { success = true, message = "Delete Success" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { success = false, error = ex.ToString() });
                }
            }
        }

        public IActionResult Borrow(BorrowRegist borrowLst)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //update RegistForm
                    var registForm = _db.DeviceRegistrations.FirstOrDefault(p => p.RegistId == borrowLst.RegistID);
                    if (registForm != null)
                    {
                        registForm.Status = "Đã Mượn";
                        registForm.ActualBorrowDate = DateTime.Now;
                    }

                    //Add Detail Regist & set trạng thái trong kho của thiết bị = false
                    foreach (var item in borrowLst.ListItemID)
                    {
                        var find = _db.Items.FirstOrDefault(p => p.ItemId == item);
                        if (find != null)
                        {
                            var detail = new DetailRegist();
                            detail.RegistId = borrowLst.RegistID;
                            detail.DeviceId = find.DeviceId;
                            detail.ItemId = find.ItemId;
                            detail.BeforeStatus = find.Status;
                            find.IsStored = false;

                            _db.DetailRegists.Add(detail);
                        }
                    }
                    _db.SaveChanges();

                    //Update ConfirmQty List Device Regist
                    var lstDeviceRegist = _db.ListDeviceRegists.Where(p => p.RegistId == registForm.RegistId).ToList();
                    foreach (var item in lstDeviceRegist)
                    {
                        var lst = _db.DetailRegists.Where(p => p.RegistId == item.RegistId && p.DeviceId == item.DeviceId).ToList();

                        var confirmQty = lst.Count();

                        item.ConfirmQuantity = confirmQty;
                    }

                    _db.SaveChanges();
                    transaction.Commit();
                    return new OkObjectResult(new { success = true, message = "Cập nhật thành công" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new OkObjectResult(new { success = true, message = "Cập nhật thất bại" });
                }
            }
        }

        public IActionResult Return(ReturnRegist returnLst)
        {
            using(var transaction= _db.Database.BeginTransaction())
            {
                try
                {
                    //update RegistForm
                    var registForm = _db.DeviceRegistrations.FirstOrDefault(p => p.RegistId == returnLst.RegistID);
                    if (registForm != null)
                    {
                        registForm.Status = "Đã Trả";
                        registForm.ActualReturnDate = DateTime.Now;
                    }

                    //Add Detail Regist & set trạng thái trong kho của thiết bị = false
                    foreach (var item in returnLst.ListItem)
                    {
                        var find = _db.Items.FirstOrDefault(p => p.ItemId == item.ItemID);
                        if(find != null)
                        {
                            var curItem = _db.DetailRegists.FirstOrDefault(p => p.RegistId == returnLst.RegistID && p.ItemId == item.ItemID);
                            curItem.AfterStatus = item.CurrentStatus;
                            find.IsStored = true;
                        }
                    }

                    _db.SaveChanges();
                    transaction.Commit();
                    return new OkObjectResult(new { success = true, message = "Cập nhật thành công" });
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new {success = false, error ="Cập nhật thất bại"+ ex.Message});
                }
            }
        }

    }
}
