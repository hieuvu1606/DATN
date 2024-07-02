using DATN.Models;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DATN.Services.CategoryService
{
    public interface ICategoryService
    {
        IActionResult AddCategory(string descr);
        IActionResult GetAll([FromQuery] PaginationFilter filter);
        IActionResult Update(Category cate);
        IActionResult GetByID(int id);
        IActionResult Delete(int id);
    }
}
