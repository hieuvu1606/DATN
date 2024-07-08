using DATN.CustomModels;
using DATN.Models;
using DATN.Utils;
using DATN.Utils.Response;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DATN.Services.PenaltyTicketService
{
    public class PenaltyTicketService
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

        public IActionResult GetByID(PaginationFilter filter, int id)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);

            var lst = _db.PenaltyTickets.Where(p => p.PenaltyId == id).Skip((validFilter.page - 1) * validFilter.pageSize)
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
                    ticket.Status = false;
                    ticket.Proof = "";
                    ticket.TotalFine = newTicket.TotalFine;

                    _db.SaveChanges();

                    foreach (DetailsPenaltyTicket newDetail in newTicket.Details)
                    {
                        var detail = new DetailsPenaltyTicket();
                        detail.PenaltyId = ticket.PenaltyId;
                        detail.RegistId = newTicket.RegistId;

                    }


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


    }
}
