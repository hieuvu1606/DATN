﻿using DATN.Models;
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
        #region Device Regist
        public IActionResult GetAll(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);

            var lst = _db.DeviceRegistrations.Skip((validFilter.page - 1) * validFilter.pageSize)
                 .Take(validFilter.pageSize).ToList();

            var count = _db.DeviceRegistrations.Count();

            return new OkObjectResult(new PagedResponse<List<DeviceRegistration>>(lst, validFilter.page, validFilter.pageSize, count, true));
        }

        public IActionResult GetByUser(PaginationFilter filter, int userID)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);

            var lst = _db.DeviceRegistrations.Where(p => p.UserId == userID).Skip((validFilter.page - 1) * validFilter.pageSize)
                 .Take(validFilter.pageSize).ToList();

            var count = _db.DeviceRegistrations.Where(p => p.UserId == userID).Count();

            return new OkObjectResult(new PagedResponse<List<DeviceRegistration>>(lst, validFilter.page, validFilter.pageSize, count, true));
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
                        ManagerId = null,
                        RegistDate = DateTime.Now,
                        BorrowDate = regist.BorrowDate,
                        ReturnDate = regist.ReturnDate,
                        Status = "Chờ Duyệt",
                        WarehouseId = regist.WarehouseID,
                        ActualBorrowDate = null,
                        ActualReturnDate = null,
                        Reason = "",
                        Notice = regist.Notice,
                    };

                    _db.DeviceRegistrations.Add(deviceRegist);
                    _db.SaveChanges();

                    foreach (var lstDevice in regist.ListRegist)
                    {
                        var newList = new ListDeviceRegist
                        {
                            RegistId = deviceRegist.RegistId,
                            DeviceId = lstDevice.DeviceID,
                            BorrowQuantity = lstDevice.Quantity,
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
                    var check = _db.DeviceRegistrations.FirstOrDefault(p => p.RegistId == registForm.RegistId);
                    if (check == null) { return new BadRequestObjectResult(new { success = false, message = "Không tìm thấy Phiếu Đăng Ký" }); }

                    #region Check Số lượng mượn
                    var checkin = new List<int>();
                    foreach (var item in borrowLst.ListItemID)
                    {
                        var find = _db.Items.FirstOrDefault(p => p.ItemId == item).DeviceId;
                        checkin.Add(find);
                    }


                    string error = string.Empty;
                    var checkQty = _db.ListDeviceRegists.Where(p => p.RegistId == registForm.RegistId).ToList();
                    foreach(var item in checkQty)
                    {
                        if (item.ConfirmQuantity == item.BorrowQuantity)
                        {
                            var deviceDescr = _db.Devices.Where(d => d.DeviceId == item.DeviceId).Select(d => d.Descr).FirstOrDefault();
                            error += $"Thiết bị {deviceDescr} đã mượn đủ.\n";
                        }
                    }

                    if(error == string.Empty)
                    {
                        foreach (var item in checkQty)
                        {
                            var lst = _db.ListDeviceRegists.Where(p => p.RegistId == item.RegistId && p.DeviceId == item.DeviceId).ToList();

                            var deviceQty = lst.Count();

                            var deviceCheckinQty = checkin.Count(id => id == item.DeviceId);

                            if (item.ConfirmQuantity == item.BorrowQuantity)
                            {
                                var deviceDescr = _db.Devices.Where(d => d.DeviceId == item.DeviceId).Select(d => d.Descr).FirstOrDefault();
                                error += $"Thiết bị {deviceDescr} đã mượn đủ.\n";
                            }

                            if (deviceCheckinQty > deviceQty)
                            {
                                var deviceDescr = _db.Devices.Where(d => d.DeviceId == item.DeviceId).Select(d => d.Descr).FirstOrDefault();
                                error += $"Thiết bị {deviceDescr} không được mượn quá {item.BorrowQuantity}.\n";
                            }

                        }
                    }                

                    if (error != "")
                        return new BadRequestObjectResult(new {success = false, error = $"{error}"});
                    #endregion

                    //Add Detail Regist & set trạng thái trong kho của thiết bị = false
                    foreach (var item in borrowLst.ListItemID)
                    {
                        var find = _db.Items.FirstOrDefault(p => p.ItemId == item);
                        if (find != null)
                        {
                            var detail = new DetailRegist();
                            detail.RegistId = borrowLst.RegistID;
                            detail.DeviceId = find.DeviceId;
                            detail.ItemId = item;
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
                    var fineLst = new List<DetailsPenaltyTicket>();
                    var fineCheck = false;
                    var ticket = new PenaltyTicket();


                    #region Update RegistForm
                    var registForm = _db.DeviceRegistrations.FirstOrDefault(p => p.RegistId == returnLst.RegistID);
                    if (registForm != null)
                    {
                        registForm.Status = "Đã Trả";
                        registForm.ActualReturnDate = DateTime.Now;
                    }
                    else
                    {
                        return new BadRequestObjectResult(new {success = false, error = "Không tìm thấy phiếu đăng ký này"});
                    }

                    DateTime? registReturnDate = registForm.ActualReturnDate;

                    DateOnly tempDate = registForm.ReturnDate;
                    DateTime actualReturnDate = tempDate.ToDateTime(TimeOnly.MinValue);

                    TimeSpan? days = actualReturnDate - registReturnDate;
                    if (days.Value.TotalDays < 0)
                    {
                        ticket.ManagerId = 0;
                        ticket.Status = false;
                        ticket.Proof = "";
                        ticket.TotalFine = 0;
                        ticket.Status = false;
                        _db.PenaltyTickets.Add(ticket);
                        _db.SaveChanges();

                        fineCheck = true;
                        var fine = new DetailsPenaltyTicket()
                        {
                            LineRef = -1,
                            Descr = $"Nộp trễ {Math.Floor(Math.Abs(days.Value.TotalDays))} ngày với ngày đăng ký trả",
                            Fine = 0,
                            PenaltyId = ticket.PenaltyId,
                            RegistId = returnLst.RegistID
                        };
                        fineLst.Add(fine);
                    }
                    #endregion

                    #region Tạo danh sách phạt
                    //Add Detail Regist & set trạng thái trong kho của thiết bị = false
                    //var fineTicket = _db
                    foreach (var item in returnLst.ListItem)
                    {
                        string descr = string.Empty;
                        var find = _db.Items.FirstOrDefault(p => p.ItemId == item.ItemId);
                        if(find != null)
                        {
                            find.IsStored = true;
                            var curItem = _db.DetailRegists.FirstOrDefault(p => p.RegistId == returnLst.RegistID && p.ItemId == item.ItemId);
                            curItem.AfterStatus = item.AfterStatus;
                            if(item.AfterStatus == "Hỏng" || item.AfterStatus == "Mất")
                            {

                                if (fineCheck == false)
                                {
                                    ticket.ManagerId = 0;
                                    ticket.Status = false;
                                    ticket.Proof = "";
                                    ticket.TotalFine = 0;
                                    ticket.Status = false;
                                    _db.PenaltyTickets.Add(ticket);
                                    _db.SaveChanges();
                                }
                                fineCheck = true;

                                var deviceDescr = (from i in _db.Items
                                                   join d in _db.Devices on i.DeviceId equals d.DeviceId
                                                   where i.ItemId == curItem.ItemId
                                                   select d.Descr).FirstOrDefault();

                                if(item.AfterStatus == "Mất")
                                {
                                    find.IsStored = false;
                                    descr = $"Mất Thiết Bị {deviceDescr} Mã {curItem.ItemId}";
                                    
                                }
                                else
                                {
                                    descr = $"Hỏng Thiết Bị {deviceDescr} Mã {curItem.ItemId}";
                                }

                                var fine = new DetailsPenaltyTicket();
                                fine.Descr = descr;
                                fine.LineRef = curItem.ItemId;
                                fine.Fine = 0;
                                fine.PenaltyId = ticket.PenaltyId;
                                fine.RegistId = returnLst.RegistID;


                                fineLst.Add(fine);
                            }
                        }

                        var findItem = _db.Items.FirstOrDefault(p => p.ItemId == item.ItemId);
                        findItem.Status = item.AfterStatus;
                    }
                    #endregion

                    _db.DetailsPenaltyTickets.AddRange(fineLst);

                    if(fineCheck == true)
                    {
                        var regist = _db.DeviceRegistrations.FirstOrDefault(p => p.RegistId == returnLst.RegistID);
                        regist.IsFine = true;
                    }

                    _db.SaveChanges();
                    transaction.Commit();
                    if(fineCheck ==  true)
                        return new OkObjectResult(new { fine = true, message = $"Phiếu phạt {ticket.PenaltyId} đã được tạo cho phiếu đăng ký {returnLst.RegistID}" });
                    else
                        return new OkObjectResult(new { fine = false,  success = true, message = "Trả thiết bị thành công" });
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new {success = false, error = "Trả thiết bị thất bại" + ex.Message});
                }
            }
        }
        #endregion

        #region List Device Regist
        public IActionResult GetList(int registID)
        {
            var devices = _db.ListDeviceRegists.Where(p => p.RegistId == registID).ToList();

            var details = _db.DetailRegists.Where(p => p.RegistId == registID).ToList();

            var deviceIds = devices.Select(d => d.DeviceId).Distinct().ToList();

            var deviceDescriptions = _db.Devices.Where(d => deviceIds.Contains(d.DeviceId)).ToDictionary(d => d.DeviceId, d => d.Descr);

            // Build the result
            var result = devices.Select(device => new GetListDetail
            {
                DeviceDescr = deviceDescriptions.ContainsKey(device.DeviceId) ? deviceDescriptions[device.DeviceId] : string.Empty,
                DeviceRegist = device,
                ListDetails = details.Where(detail => detail.DeviceId == device.DeviceId)
                    .Select(detail => new CustomDetail
                    {
                        ItemId = detail.ItemId,
                        DeviceDescr = deviceDescriptions.ContainsKey(detail.DeviceId) ? deviceDescriptions[detail.DeviceId] : string.Empty,
                        RegistId = detail.RegistId,
                        DeviceId = detail.DeviceId,
                    })
                    .ToList()
            }).ToList();

            return new OkObjectResult(result);
        }
        #endregion
    }
}
