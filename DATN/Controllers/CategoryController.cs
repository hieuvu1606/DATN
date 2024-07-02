using DATN.Models;
using DATN.Services.CategoryService;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DATN.Controllers
{
    [Route("category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController( ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] string descr)
        {
           return _categoryService.AddCategory(descr);
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll([FromQuery]PaginationFilter filter)
        {
            return _categoryService.GetAll(filter);
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public IActionResult GetByID(int id)
        {
            return _categoryService.GetByID(id);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            return _categoryService.Delete(id);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update([FromBody] Category cate)
        {
            return _categoryService.Update(cate);
        }
    }
}
