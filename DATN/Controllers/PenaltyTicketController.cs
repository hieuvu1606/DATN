﻿using DATN.CustomModels;
using DATN.Services.PenaltyTicketService;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Controllers
{
    [Route("penalty")]
    [ApiController]
    public class PenaltyTicketController : Controller
    {
        private readonly IPenaltyTicketService _penaltyTicketService;

        public PenaltyTicketController(IPenaltyTicketService penaltyTicketService)
        {
            _penaltyTicketService = penaltyTicketService;
        }

       /* [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] PostPenalty newTicket)
        {
            return _penaltyTicketService.Create(newTicket);
        }*/

        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll([FromQuery] PaginationFilter filter)
        {
            return _penaltyTicketService.GetAll(filter);
        }

        [HttpGet]
        [Route("getByID/{id}")]
        public IActionResult GetByID(int penaltyID)
        {
            return _penaltyTicketService.GetByID(penaltyID);
        }

        [HttpGet]
        [Route("getByUserID/{userId}")]
        public IActionResult GetByUserID([FromQuery] PaginationFilter filter, int userId)
        {
            return _penaltyTicketService.GetByUserID(filter, userId);
        }

        [HttpPost]
        [Route(("updateStatus/{penaltyID}&{status}"))]
        public IActionResult UpdateStatus(int penaltyID, bool status)
        {
            return _penaltyTicketService.UpdateStatus(penaltyID, status);
        }

        [HttpGet]
        [Route("getDetail/{ticketID}&{check}")]
        public IActionResult GetDetail(int check, int ticketID)
        {
            return _penaltyTicketService.GetDetail(check, ticketID);
        }

        [HttpPost]
        [Route("updateDetail")]
        public IActionResult UpdateDetail([FromBody] PostPenalty penalty)
        {
            return _penaltyTicketService.UpdateDetail(penalty);
        }

    }
}
