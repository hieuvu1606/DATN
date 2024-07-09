using DATN.CustomModels;
using DATN.Models;
using DATN.Utils;
using DATN.Utils.Response;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DATN.Services.PenaltyTicketService
{
    public class PenaltyTicketService : IPenaltyTicketService
    {
        private readonly DeviceContext _db;
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

        public IActionResult GetDetailByID(PaginationFilter filter, int penaltyID)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);

            var lst = _db.PenaltyTickets.Where(p => p.PenaltyId == penaltyID).Skip((validFilter.page - 1) * validFilter.pageSize)
                        .Take(validFilter.pageSize).ToList();

            var count = lst.Count();

            return new OkObjectResult(new PagedResponse<List<PenaltyTicket>>(lst, validFilter.page, validFilter.pageSize, count, true));
        }

        public IActionResult GetByUserID (PaginationFilter filter,int userId)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);


            var lst  = (from pt in _db.PenaltyTickets
                        join dpt in _db.DetailsPenaltyTickets on pt.PenaltyId equals dpt.PenaltyId
                        join drg in _db.DetailRegists on new { dpt.RegistId, dpt.DeviceId, dpt.ItemId } equals new { drg.RegistId, drg.DeviceId, drg.ItemId }
                        join dr in _db.DeviceRegistrations on drg.RegistId equals dr.RegistId
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

        public IActionResult Create(CreatePenalty newTicket)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var ticket = new PenaltyTicket();
                    ticket.ManagerId = newTicket.ManagerId;
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
                        detail.RegistId = newTicket.RegistId;
                        detail.ItemId = newDetail.ItemID;
                        detail.DeviceId = _db.Items.Where(p => p.ItemId == newDetail.ItemID).Select(p => p.DeviceId).FirstOrDefault();
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
        }

        public IActionResult UpdateStatus (int penaltyID)
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
                    findTicket.Status = true;
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
    }
}
