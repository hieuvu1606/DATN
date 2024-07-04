using DATN.Models;
using Microsoft.AspNetCore.Mvc;
using DATN.Models;
using DATN.CustomModels;
namespace DATN.Services.RegistDevice
{
    public class RegistDeviceService
    {
        private readonly DeviceContext _db;
        
        public RegistDeviceService(DeviceContext db)
        {
            _db = db;
        }

        public IActionResult GetAll()
        {
            var lst = _db.DeviceRegistrations.ToList();
            return new OkObjectResult(lst);
        }

        public IActionResult GetByUser(int userID)
        {
            var lst = _db.DeviceRegistrations.Where(p => p.UserId == userID).ToList();
            return new OkObjectResult(lst);
        }

        public IActionResult GetById(int id)
        {
            var regist = _db.DeviceRegistrations.FirstOrDefault(p => p.RegistId == id);
            return new OkObjectResult(regist);
        }

        public IActionResult Create(RegistForm regist)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var deviceRegist = new DeviceRegistration
                    {
                        UserId = regist.UserID,
                        Proof = null,
                        RegistDate = DateTime.Now,
                        BorrowDate = regist.BorrowDate,
                        ReturnDate = regist.ReturnDate,
                        Status = "Chờ Duyệt",
                        WarehouseId = regist.WarehouseID,
                        ActualBorrowDate = null,
                        ActualReturnDate = null,
                        Reason = ""
                    };

                    _db.DeviceRegistrations.Add(deviceRegist);
                    _db.SaveChanges();

                    foreach (ListDeviceRegist lstDevice in deviceRegist.ListDeviceRegists)
                    {
                        var newList = new ListDeviceRegist
                        {
                            RegistId = deviceRegist.RegistId,
                            DeviceId = lstDevice.DeviceId,
                            BorrowQuantity = lstDevice.BorrowQuantity,
                            ConfirmQuantity = 0
                        };
                        _db.ListDeviceRegists.Add(newList);
                    }
                    _db.SaveChanges();

                    transaction.Commit();
                    return new OkObjectResult(new { success = true, message = "Create New Success" });

                }catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { success = false, error = $"Can't Create New Regist {ex.Message}"  });
                }
            }

        }




    }
}
