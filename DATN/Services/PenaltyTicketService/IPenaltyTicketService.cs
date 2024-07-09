using DATN.CustomModels;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Services.PenaltyTicketService
{
    public interface IPenaltyTicketService
    {
        IActionResult Create(CreatePenalty newTicket);
        IActionResult GetAll(PaginationFilter filter);
        IActionResult GetDetailByID(PaginationFilter filter, int id);
        IActionResult GetByUserID(PaginationFilter filter, int userId);
        IActionResult UpdateStatus(int penaltyID);
    }
}
