using DATN.CustomModels;
using DATN.Models;
using DATN.Utils;
using DATN.Utils.Response;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DATN.Services.PenaltyTicketService
{
    public class PenaltyTicketService : IPenaltyTicketService
    {
        private readonly DeviceContext _db;

        #region Penalty
        public PenaltyTicketService(DeviceContext db)
        {
            _db = db;
        }

        public IActionResult GetAll(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);

            var lst = _db.PenaltyTickets.Skip((validFilter.page - 1) * validFilter.pageSize)
                        .Take(validFilter.pageSize).ToList();

            var count = lst.Count();

            return new OkObjectResult(new PagedResponse<List<PenaltyTicket>>(lst, validFilter.page, validFilter.pageSize, count, true));
        }

        public IActionResult GetByID(int penaltyID)
        {

            var ticket = _db.PenaltyTickets.FirstOrDefault(p => p.PenaltyId == penaltyID);

            return new OkObjectResult(ticket);
        }

        public IActionResult GetByUserID (PaginationFilter filter,int userId)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);


            var lst  = (from pt in _db.PenaltyTickets
                        join dpt in _db.DetailsPenaltyTickets on pt.PenaltyId equals dpt.PenaltyId
                        join dr in _db.DeviceRegistrations on dpt.RegistId equals dr.RegistId
                        where dr.UserId == userId
                        select new PenaltyTicket
                        {
                            PenaltyId = pt.PenaltyId,
                            ManagerId = pt.ManagerId,
                            Proof = pt.Proof,
                            Status = pt.Status,
                            TotalFine = pt.TotalFine
                        })
                        .Skip((validFilter.page - 1) * validFilter.pageSize)
                        .Take(validFilter.pageSize)
                        .ToList();


            var count = lst.Count();

            return new OkObjectResult(new PagedResponse<List<PenaltyTicket>>(lst, validFilter.page, validFilter.pageSize, count, true));
        }

       /* public IActionResult Create(PostPenalty newTicket)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var ticket = new PenaltyTicket();
                    ticket.ManagerId = newTicket.ManagerID;
                    ticket.Status = false;
                    ticket.Proof = "";
                    ticket.TotalFine = 0;
                    _db.PenaltyTickets.Add(ticket);
                    _db.SaveChanges();

                    var totalfine = 0;
                    foreach (var newDetail in newTicket.ListPenalty)
                    {
                        var detail = new DetailsPenaltyTicket();
                        detail.PenaltyId = ticket.PenaltyId;
                        detail.RegistId = newTicket.RegistID;
                        detail.LineRef = newDetail.LineRef;
                        detail.Descr = newDetail.Descr;
                        detail.Fine = newDetail.Fine;

                        totalfine += newDetail.Fine;

                        _db.DetailsPenaltyTickets.Add(detail);
                    }

                    ticket.TotalFine = totalfine;


                    _db.SaveChanges();

                    transaction.Commit();
                    return new OkObjectResult(new { success = true, message = "Tạo Phiếu Phạt Thành Công" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { success = true, message = "Tạo Phiếu Phạt Thất Bại" + ex.ToString()});
                }
            }
        }*/
        public IActionResult UpdateStatus (int penaltyID, bool status)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var findTicket = _db.PenaltyTickets.FirstOrDefault(p => p.PenaltyId == penaltyID);
                    if(findTicket == null)
                    {
                        return new BadRequestObjectResult(new {success = false, error = "Không tìm thấy phiếu phạt"});
                    }
                    findTicket.Status = status;
                    _db.SaveChanges();
                    transaction.Commit();
                    return new OkObjectResult(new { success = true, message = "Cập nhật phiếu phạt thành công" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { success = false, error = ex.Message });
                }
            }
        }
        #endregion

        #region Detail Penalty
        public IActionResult GetDetail(int id)
        {
            ////var validFilter = new PaginationFilter(filter.page, filter.pageSize);

            //var lst = _db.DetailsPenaltyTickets.Where(p => p.PenaltyId == id || p.RegistId == id).Skip((validFilter.page - 1) * validFilter.pageSize)
            //            .Take(validFilter.pageSize).ToList();

            //var count = lst.Count();

            //return new OkObjectResult(new PagedResponse<List<DetailsPenaltyTicket>>(lst, validFilter.page, validFilter.pageSize, count, true));

            var lst = _db.DetailsPenaltyTickets.Where(p => p.PenaltyId == id || p.RegistId == id).ToList();

            return new OkObjectResult(lst);
        }

        public IActionResult UpdateDetail(PostPenalty lstDetail)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var totalFine = 0;
                    var registID = lstDetail.RegistID;
                    var penaltyID = lstDetail.PenaltyID;
                    var managerID = lstDetail.ManagerID;

                    foreach(var detail in lstDetail.ListPenalty)
                    {
                        var find = _db.DetailsPenaltyTickets.FirstOrDefault(p => p.RegistId == registID && p.PenaltyId == penaltyID && p.LineRef == detail.LineRef);

                        find.Fine = detail.Fine;
                        totalFine += detail.Fine;
                    }

                    var ticket = _db.PenaltyTickets.FirstOrDefault(p => p.PenaltyId == penaltyID);
                    ticket.TotalFine = totalFine;

                    _db.SaveChanges();

                    transaction.Commit();
                    return new OkObjectResult(new {success = true, message = "Cập Nhật phiếu phạt thành công" });
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new {success = false, error = ex.ToString() });
                }
            }
        }
        #endregion
    }
}
