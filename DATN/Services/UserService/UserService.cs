﻿using DATN.Models;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using DATN.CustomModels;
using DATN.Services.Email;
using System.Diagnostics;
using System.Text;
using ExcelDataReader;
using System.Transactions;
using DATN.Utils.Response;
namespace DATN.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DeviceContext _db;
        private readonly IEmailService _emailService;
        public UserService(DeviceContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        public IActionResult Register(RegisterUser user)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _emailService.Connect();

                    var (account, password) = AccountGenerator.GenerateAccountAndRandomPassword(user.Name, user.Surname, _db);

                    var hashedPassword = PasswordHasher.HashPassword(password);

                    var newUser = new User();

                    newUser.Surname = user.Surname;
                    newUser.Name = user.Name;
                    newUser.PhoneNumber = user.PhoneNumber;
                    newUser.Email = user.Email;
                    newUser.RoleId = user.RoleId;
                    newUser.CitizenId = user.CitizenId;

                    newUser.Password = hashedPassword;
                    newUser.Account = account;
                    newUser.RandomPassword = true;

                    _db.Users.Add(newUser);
                    _db.SaveChanges();
                    transaction.Commit();

                    var subject = "Account Information DATN";
                    var message = "Account: " + account + "\nPassword: " + password;
                    _emailService.SendEmail(newUser.Email, subject, message);

                    _emailService.Dispose();

                    return new OkObjectResult(new { message = "success" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { error = "An error occurred during registration.", ExceptionMessage = ex.Message });
                }
            }         
        }

        public IActionResult Login(Login login)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(p => p.Account == login.Account);

                if (user == null)
                {
                    return new BadRequestObjectResult(new { error = "Account does not exist" });
                }
                else
                {
                    if (PasswordHasher.VerifyPassword(login.Password, user.Password))
                    {
                        return new OkObjectResult(new {success = true, message = "Login Success"});
                    }
                    else
                        return new UnauthorizedResult();
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { error = "", ex.Message });
            }
        }

        public IActionResult ChangePassword(ChangePassword change)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var user = _db.Users.FirstOrDefault(u => u.Account == change.Account);
                    if (user == null)
                    {
                        return new BadRequestObjectResult(new { error = "User does not exist" });
                    }
                    else
                    {
                        if (PasswordHasher.VerifyPassword(change.OldPassword, user.Password))
                        {
                            var hashedPassword = PasswordHasher.HashPassword(change.NewPassword);

                            user.Password = hashedPassword;

                            _db.SaveChanges();

                            transaction.Commit();
                            return new OkResult();
                        }
                        else
                        {
                            return new BadRequestObjectResult(new { success = false, error = "Old Password Is Not Correct" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { error = "", ex.Message });
                }
            }            
        }

        public IActionResult GetBasicInfoUsers(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);
            var lst = _db.Users
                 .Join(_db.Roles,user => user.RoleId, role => role.RoleId,
                        (user, role) => new UserBasicInfo(
                           user.UserId, user.Surname, user.Name, user.Email, user.PhoneNumber, role.Descr))
                 .Skip((validFilter.page - 1) * validFilter.pageSize)
                 .Take(validFilter.pageSize).ToList();

            var count = _db.Users.Count();

            return new OkObjectResult(new PagedResponse<List<UserBasicInfo>>(lst, validFilter.page, validFilter.pageSize, count, true));
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var item = _db.Users.Where(p => p.UserId == id).FirstOrDefault();
                if(item != null)
                {
                    _db.Users.Remove(item);
                }
                return new OkObjectResult(new { message = "Success" });
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(new { error = ex.Message});
            }
           
        }

        public IActionResult ResetPassword(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var user = _db.Users.FirstOrDefault(p => p.UserId == id);

                    if (user == null)
                    {
                        return new BadRequestObjectResult(new { error = "Can't Found User" });
                    }
                    var (newAccount, newPassword) = AccountGenerator.GenerateAccountAndRandomPassword("","", _db);

                    user.Password = newPassword;
                    _db.SaveChanges();
                    transaction.Commit();
                    _emailService.Connect();
                    string subject = "Reset Password Success";
                    string message = "New Password: " + newPassword.ToString();
                    _emailService.SendEmail(user.Email, subject, message);
                    return new OkObjectResult(new { success = true, message = "Reset Password Success" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new {error = ex.Message});
                }
            }       
        }

        public IActionResult UpdateRole (int userid, int roleID)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var user = _db.Users.FirstOrDefault(p => p.UserId == userid);
                    if (user == null)
                    {
                        return new BadRequestObjectResult(new {success = false, error = "Can Not Found User" });
                    }

                    user.RoleId = roleID;
                    _db.SaveChanges();

                    transaction.Commit();
                    return new OkObjectResult(new { success = true, message = "Update Role Success" });
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new {success = false, error = ex.ToString()});
                }
            }
        }



        //public async Task<IActionResult> UploadExcel(IFormFile file)
        //{
        //    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        //    if (file != null && file.Length > 0)
        //    {
        //        var uploadsFolder = $"{Directory.GetCurrentDirectory()}";

        //        if (!Directory.Exists(uploadsFolder))
        //        {
        //            Directory.CreateDirectory(uploadsFolder);
        //        }

        //        var filePath = Path.Combine(uploadsFolder, file.FileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }

        //        using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
        //        {
        //            using (var reader = ExcelReaderFactory.CreateReader(stream))
        //            {
        //                do
        //                {
        //                    bool isHeaderSkipped = false;

        //                    while (reader.Read())
        //                    {
        //                        if (!isHeaderSkipped)
        //                        {
        //                            isHeaderSkipped = true;
        //                            continue;
        //                        }


        //                    }
        //                } while (reader.NextResult());
        //            }
        //        }
        //    }
        //}
    }
}

