using DATN.Models;
using Microsoft.AspNetCore.Mvc;
using DATN.Utils;
using System.Web.Http;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using DATN.CustomModels;
using DATN.Utils.Response;

namespace DATN.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DeviceContext _db;
        public CategoryService(DeviceContext db)
        {
            _db = db;
        }

        public IActionResult AddCategory(string descr)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var find = _db.Categories.FirstOrDefault(p => p.Descr == descr);
                    if (descr == null || descr == "" || find != null)
                    {
                        return new BadRequestObjectResult(new { error = "Invalid Category name" });
                    }
                    else
                    {
                        var category = new Category();
                        category.Descr = descr;
                        _db.Categories.Add(category);
                        _db.SaveChanges();

                        transaction.Commit();
                        return new OkObjectResult(new { message = "Success" });
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { error = "", ex.Message });
                }
            }           
        }

        public IActionResult GetAll([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.page, filter.pageSize);

            var lst = _db.Categories.Skip((validFilter.page - 1) * validFilter.pageSize)
                 .Take(validFilter.pageSize).ToList();

            var count = _db.Categories.Count();

            return new OkObjectResult(new PagedResponse<List<Category>>(lst, validFilter.page, validFilter.pageSize, count, true));
        }

        public IActionResult GetByID(int id)
        {
            var cate = _db.Categories.FirstOrDefault(p => p.CategoryId == id);
            return new OkObjectResult(cate);
        }

        public IActionResult Update(Category cate)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {

                    var find = _db.Categories.FirstOrDefault(p => p.Descr == cate.Descr);
                    if (cate.Descr == null || cate.Descr == "" || find != null)
                    {
                        return new BadRequestObjectResult(new { error = "Invalid Category name" });
                    }
                    else
                    {
                        var findID = _db.Categories.FirstOrDefault(p => p.CategoryId == cate.CategoryId);
                        if (findID != null)
                        {
                            findID.Descr = cate.Descr;
                            _db.SaveChanges();
                            transaction.Commit();
                        }

                        return new OkObjectResult(new { error = "Success" });
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new { error = "", ex.Message });
                }
            }             
        }

        public IActionResult Delete(int id)
        {
            var cate = _db.Categories.FirstOrDefault(p => p.CategoryId == id);
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (cate == null)
                    {
                        return new BadRequestObjectResult(new { success = false, error = "Can't Found Category" });
                    }
                    _db.Categories.Remove(cate);
                    _db.SaveChanges();
                    transaction.Commit();
                    return new OkObjectResult(new {success = true, message = "Category And All Data References Have Been Deleted"});
                }catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new {success = false, error = "Can't Cascade Category"+ex.Message});
                }
               
            }
        }
    }
}
