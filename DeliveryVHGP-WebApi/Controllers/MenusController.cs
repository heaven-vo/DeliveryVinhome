using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.Controllers
{
    [Route("api/v1/menus")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMenuRepository menuRepository;

        public MenusController(IMenuRepository menuRepository)
        {
            this.menuRepository = menuRepository;
        }

        // GET: api/Menus
        /// <summary>
        /// Get list menus with products inside by realtime
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<MenuView>>> GetMenus()
        {
            return Ok(await menuRepository.GetListMenuNow());
        }

        // GET: api/Menus
        /// <summary>
        /// Get list name of  menus by realtime
        /// </summary>
        [HttpGet("name")]
        public async Task<ActionResult<List<MenuView>>> GetMenusName()
        {
            return Ok(await menuRepository.GetListMenuName());
        }

        /// <summary>
        /// Get list menus in realtime by modeId
        /// </summary>
        [HttpGet("filter")]
        public async Task<ActionResult<List<MenuView>>> GetMenuByModeId(string modeId)
        {
            return Ok(await menuRepository.GetListMenuByMode(modeId));
        }

        /// <summary>
        /// Get list products in a menu
        /// </summary>
        [HttpGet("{id}/products")]
        public async Task<ActionResult<List<ProductViewInList>>> GetAllProductInMenu(string menuId, int page, int pageSize)
        {
            return Ok(await menuRepository.GetListProductInMenu(menuId, page, pageSize));
        }

        /// <summary>
        /// Get list products in a menu and a store
        /// </summary>
        [HttpGet("{id}/products/byStoreId")]
        public async Task<ActionResult<List<ProductViewInList>>> GetAllProductInMenuByStoreId(string menuId, string storeId, int page, int pageSize)
        {
            return Ok(await menuRepository.GetListProductInMenuByStoreId(storeId, menuId, page, pageSize));
        }

        /// <summary>
        /// Get list products in a menu and a category
        /// </summary>
        [HttpGet("{id}/products/byCategoryId")]
        public async Task<ActionResult<List<ProductViewInList>>> GetAllProductInMenuByCategoryId(string menuId, string categoryId, int page, int pageSize)
        {
            return Ok(await menuRepository.GetListProductInMenuByCategoryId(categoryId, menuId, page, pageSize));
        }
    }
}
