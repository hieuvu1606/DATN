﻿using DATN.CustomModels;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Services.RegistDevice
{
    public interface IRegistDeviceService
    {
        IActionResult Borrow(BorrowRegist borrowLst);
        IActionResult Create(RegistForm regist);
        IActionResult Delete(int registID);
        IActionResult GetAll(PaginationFilter filter);
        IActionResult GetById(int id);
        IActionResult GetByUser(PaginationFilter filter, int userID);
        IActionResult GetList(int registID);
        IActionResult Return(ReturnRegist returnLst);
        IActionResult UpdateStatus(UpdateStatusRegist updateStatus);
    }
}
