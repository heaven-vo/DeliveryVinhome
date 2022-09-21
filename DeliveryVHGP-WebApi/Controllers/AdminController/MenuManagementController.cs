using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP_WebApi.Controllers.AdminController
{
    [Route("api/v1/menu-management/menus")]
    [ApiController]
    public class MenuManagementController : ControllerBase
    {
        private readonly IMenuRepository menuRepository;

        public MenuManagementController(IMenuRepository menuRepository)
        {
            this.menuRepository = menuRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MenuDto>> CreatNewMenu(MenuDto menu)
        {
            try
            {
                await menuRepository.CreatNewMenu(menu);
            }
            catch
            {
                return Conflict();
            }
            return Ok(menu);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<MenuDto>> UpdateMenu(string menuId, MenuDto menu)
        {
            if (menuId == null || menu.Id == null || menuId != menu.Id)
            {
                return BadRequest();
            }
            try
            {
                await menuRepository.UpdateMenu(menuId, menu);
            }
            catch
            {
                return Conflict();
            }
            return Ok(menu);
        }

    }
}
