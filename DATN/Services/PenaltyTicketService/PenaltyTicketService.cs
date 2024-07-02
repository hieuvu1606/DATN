using DATN.Models;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Services.PenaltyTicketService
{
    public class PenaltyTicketService
    {
        private readonly DeviceContext _db;
        public PenaltyTicketService(DeviceContext db)
        {
            _db = db;
        }

        public IActionResult GetAll()
        {
            var lst = _db.PenaltyTickets.ToList();
            return new OkObjectResult(lst);
        }

        //public IActionResult Create()
        //{
        //    var lst = _db.Pen
        //}

        //public IActionResult() { }

        //public IActionResult() { }

        //public IActionResult() { }

        //public IActionResult() { }

        //public IActionResult() { }


    }
}
