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
        [HttpGet("menuId")]
        public async Task<ActionResult<MenuDto>> GetMenuDetail(string menuId)
        {
            try
            {
                var detail = await menuRepository.GetMenuDetail(menuId);
                return Ok(detail);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
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
        [HttpPut("{menuId}")]
        public async Task<ActionResult<MenuDto>> UpdateMenu(string menuId, MenuDto menu)
        {
            if (menuId == null)
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
