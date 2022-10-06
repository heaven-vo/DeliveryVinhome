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
        public async Task<ActionResult> GetMenuDetail(string menuId)
        {
            try
            {
                //List<string> listA = new List<string>() {"1", "3", "4"};
                //List<string> listB = new List<string>() { "3", "4", "8", "9" };
                //var listFinal = listA.Intersect(listB);
                //var list = listB.Except(listFinal);
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
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
            return Ok(menu);
        }

    }
}
