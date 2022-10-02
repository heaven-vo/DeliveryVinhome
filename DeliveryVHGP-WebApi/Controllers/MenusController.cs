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
        /// Get all menu by modeId (store, admin web)
        /// </summary>
        [HttpGet("byMode")]
        public async Task<ActionResult<List<MenuView>>> GetMenusByMode(string modeId)
        {
            return Ok(await menuRepository.GetListMenuByModeId(modeId));
        }

        /// <summary>
        /// Get list menus in realtime by modeId(customer web)
        /// *Note: gb = store( group by store), cate(group by catedory)
        /// </summary>
        [HttpGet("filter")]
        public async Task<ActionResult<MenuView>> GetMenusByModeId(string modeId, string gb, int page, int pageSize)
        {
            MenuView menu = new MenuView();
            try{
                if (gb == "store")
                {
                    menu = await menuRepository.GetMenuByModeAndGroupByStore(modeId, page, pageSize);
                }
                if (gb == "cate")
                {
                    menu = await menuRepository.GetMenuByModeAndGroupByCategory(modeId, page, pageSize);
                }
            }
            catch
            {
                return NoContent();
            }
            return Ok(menu);           
        }

        /// <summary>
        /// Get list category in include products in menu by store id(store web)
        /// </summary>
        [HttpGet("{menuId}/filter")]
        public async Task<ActionResult<MenuView>> GetMenuByStoreId(string menuId, string storeId, int page, int pageSize)
        {
            List<CategoryStoreInMenu> menu = new List<CategoryStoreInMenu>();
            try
            {
                menu = await menuRepository.GetMenuByMenuIdAndStoreIdAndGroupByCategory(menuId, storeId, page, pageSize);
            }
            catch
            {
                return NoContent();
            }
            return Ok(menu);
        }


        /// <summary>
        /// Get list products in a menu and a store(customer web)
        /// </summary>
        [HttpGet("{menuId}/products/byStoreId")]
        public async Task<ActionResult<List<ProductViewInList>>> GetAllProductInMenuByStoreId(string menuId, string storeId, int page, int pageSize)
        {
            return Ok(await menuRepository.GetListProductInMenuByStoreId(storeId, menuId, page, pageSize));
        }

        /// <summary>
        /// Get list products in a menu and a category(customer web)
        /// </summary>
        [HttpGet("{menuId}/products/byCategoryId")]
        public async Task<ActionResult<List<ProductViewInList>>> GetAllProductInMenuByCategoryId(string menuId, string categoryId, int page, int pageSize)
        {
            return Ok(await menuRepository.GetListProductInMenuByCategoryId(categoryId, menuId, page, pageSize));
        }

        /// <summary>
        /// Get list products in store and not in a menu (store web)
        /// </summary>
        [HttpGet("{menuId}/not-products/filter")]
        public async Task<ActionResult<List<ProductViewInList>>> GetListProductNotInMenuByCategoryIdAndStoreId(string menuId,string storeId, int page, int pageSize)
        {
            return Ok(await menuRepository.GetListProductNotInMenuByCategoryIdAndStoreId(storeId, menuId, page, pageSize));

        }

        [HttpPost("{menuId}/products/join")]
        public async Task<ActionResult<ProductsInMenuModel>> AddProductsToMenu(string menuId,ProductsInMenuModel listProduct)
        {
            //try
            //{
            //    return Ok(await menuRepository.AddProductsToMenu(listProduct));
            //}
            //catch
            //{
            //    return BadRequest();
            //}
            return Ok(await menuRepository.AddProductsToMenu(listProduct));
        }
    }
}
