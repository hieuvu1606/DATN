using DATN.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DATN.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly DeviceContext _db;

        public RoleService(DeviceContext db)
        {
            _db = db;
        }

        public IActionResult Get()
        {
            var lst = _db.Roles.ToList();
            return new OkObjectResult(lst);
        }

        public IActionResult Update(FormCollection data)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var id = data["roleID"];
                    var descr = data["descr"];
                    var role = _db.Roles.FirstOrDefault(p => p.RoleId == int.Parse(id));
                    if (role == null)
                    {
                        return new BadRequestObjectResult(new { success = false, error = "Can't found Role" });
                    }

                    role.Descr = descr;
                    _db.SaveChanges();
                    transaction.Commit();
                    return new OkObjectResult(new {success = true, message = "Update Success"});
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { success = false, error = ex.Message });
                }
            }
        }
    }
}
