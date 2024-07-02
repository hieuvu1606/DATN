using DATN.CustomModels;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Services.UserService
{
    public interface IUserService
    {
        IActionResult Register(RegisterUser user);
        IActionResult ChangePassword(ChangePassword changePassword);
        IActionResult Login(Login login);
        IActionResult GetBasicInfoUsers();
    }
}
