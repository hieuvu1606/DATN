using Microsoft.AspNetCore.Mvc;

namespace DATN.Services.RoleService
{
    public interface IRoleService
    {
        IActionResult Get();
        IActionResult Update(FormCollection data);
    }
}
