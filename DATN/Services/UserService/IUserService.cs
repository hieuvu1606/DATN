﻿using DATN.CustomModels;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Services.UserService
{
    public interface IUserService
    {
        IActionResult Register(RegisterUser user);
        IActionResult ChangePassword(ChangePassword changePassword);
        IActionResult Login(Login login);
        IActionResult GetBasicInfoUsers(PaginationFilter filter);
        IActionResult Delete(int id);
        IActionResult ResetPassword(int account);
        IActionResult UpdateRole(int userid, int roleID);
    }
}
