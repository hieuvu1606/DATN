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
        IActionResult GetDetail(int id);
        IActionResult UpdateDetail(PostPenalty lstDetail);
        IActionResult UpdateStatus(int penaltyID, bool status);
    }
}
