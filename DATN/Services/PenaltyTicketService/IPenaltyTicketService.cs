using DATN.CustomModels;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Services.PenaltyTicketService
{
    public interface IPenaltyTicketService
    {
        //IActionResult Create(PostPenalty newTicket);
        IActionResult GetAll(PaginationFilter filter);
        IActionResult GetByID(int id);
        IActionResult GetByUserID(PaginationFilter filter, int userId);
        IActionResult GetDetail(PaginationFilter filter, int id);
        IActionResult UpdateStatus(int penaltyID);
    }
}
