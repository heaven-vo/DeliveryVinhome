using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/menus")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;

        public MenusController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }

        // GET: api/Menus
        /// <summary>
        /// Get all menu by modeId (store, admin web)
        /// </summary>
        [HttpGet("byMode")]
        public async Task<ActionResult<List<MenuView>>> GetMenusByMode(string modeId)
        {
            return Ok(await repository.Menu.GetListMenuByModeId(modeId));
        }
        /// <summary>
        /// Get list all store by Name with pagination
        /// </summary>
        //GET: api/v1/storeByname?pageIndex=1&pageSize=3
        [HttpGet("ByMenuId/keySearch")]
        public async Task<ActionResult> GetListStoreByName(string KeySearch, string menuId, int pageIndex, int pageSize)
        {
            return Ok(await repository.Menu.Filter(KeySearch, menuId, pageIndex, pageSize));
        }
        /// <summary>
        /// Get list product in store by name with pagination (customer web)
        /// </summary>
        //GET: api/v1/storeByBrand?pageIndex=1&pageSize=3
        [HttpGet("keySearch/store-name")]
        public async Task<ActionResult> GetListProductInStoreByName(string KeySearch, string menuId, int pageIndex, int pageSize)
        {
            return Ok(await repository.Menu.GetListProductInStoreInMenuByName(KeySearch, menuId, pageIndex, pageSize));
        }
        /// <summary>
        /// Get list category in menu in realtime by modeId(when click a mode 1 in customer web)
        /// </summary>
        [HttpGet("now/categoies")]
        public async Task<ActionResult<MenuNotProductView>> GetMenuByModeWithListCategory(string modeId)
        {
            return Ok(await repository.Menu.GetMenuByModeAndShowListCategory(modeId));
        }

        /// <summary>
        /// Get list stores in menu in realtime by modeId(when click a mode 1 in customer web)
        /// </summary>
        [HttpGet("now/storeCategories")]
        public async Task<ActionResult<StoreInMenuView>> GetMenuByModeWithListStoreCategory(string modeId, int storeCateSize, int storeSize)
        {
            return Ok(await repository.Menu.GetListStoreCateInMenuNow(modeId, storeCateSize, storeSize));
        }


        /// <summary>
        /// Get list stores in menu in realtime by modeId(when click a mode 1 in customer web)
        /// </summary>
        [HttpGet("now/stores")]
        public async Task<ActionResult<StoreInMenuView>> GetMenuByModeWithListStore(string modeId, int page, int pageSize)
        {
            return Ok(await repository.Menu.GetListStoreInMenuNow(modeId, page, pageSize));
        }


        /// <summary>
        /// Get list menus in realtime by modeId(when click a mode in customer web)
        /// *Note: gb = store(group by store), cate(group by catedory)
        /// </summary>
        [HttpGet("filter")]
        public async Task<ActionResult<MenuView>> GetMenusByModeId(string modeId, string gb, int page, int pageSize)
        {
            MenuView menu = new MenuView();
            try
            {
                if (gb == "store")
                {
                    menu = await repository.Menu.GetMenuByModeAndGroupByStore(modeId, page, pageSize);
                }
                if (gb == "cate")
                {
                    menu = await repository.Menu.GetMenuByModeAndGroupByCategory(modeId, page, pageSize);
                }
            }
            catch
            {
                return NoContent();
            }
            return Ok(menu);
        }

        /// <summary>
        /// Get list category in include products in menu by store id(when click a menu in store web)
        /// </summary>
        [HttpGet("{menuId}/filter")]
        public async Task<ActionResult<MenuView>> GetMenuByStoreId(string menuId, string storeId, int page, int pageSize)
        {
            List<CategoryStoreInMenu> menu = new List<CategoryStoreInMenu>();
            try
            {
                menu = await repository.Menu.GetMenuByMenuIdAndStoreIdAndGroupByCategory(menuId, storeId, page, pageSize);
            }
            catch
            {
                return NoContent();
            }
            return Ok(menu);
        }

        /// <summary>
        /// Get list stores in menu by categoryId(when click a cate in customer web)
        /// </summary>
        [HttpGet("{menuId}/stores/filterByCate")]
        public async Task<ActionResult<List<StoreInMenuView>>> GetListStoreInMenuFilerByCategory(string menuId, string cateId, int page, int pageSize)
        {
            List<StoreInMenuView> menu = new List<StoreInMenuView>();
            try
            {
                menu = await repository.Menu.GetListStoreInMenuFilerByCategory(menuId, cateId, page, pageSize);
            }
            catch
            {
                return NoContent();
            }
            return Ok(menu);
        }

        /// <summary>
        /// Get list products in a menu and a store(when click see all in customer web)
        /// </summary>
        [HttpGet("{menuId}/products/byStoreId")]
        public async Task<ActionResult<List<ProductViewInList>>> GetAllProductInMenuByStoreId(string menuId, string storeId, int page, int pageSize)
        {
            return Ok(await repository.Menu.GetAllProductInMenuByStoreId(storeId, menuId, page, pageSize));
        }

        /// <summary>
        /// Get list products in a menu and a category(when click see all in customer web)
        /// </summary>
        [HttpGet("{menuId}/products/byCategoryId")]
        public async Task<ActionResult<List<ProductViewInList>>> GetAllProductInMenuByCategoryId(string menuId, string categoryId, int page, int pageSize)
        {
            return Ok(await repository.Menu.GetAllProductInMenuByCategoryId(categoryId, menuId, page, pageSize));
        }

        /// <summary>
        /// Get list products in a menu and a category(when click see all in store web)
        /// </summary>
        [HttpGet("{menuId}/products/byStoreAndCategory")]
        public async Task<ActionResult<CategoryStoreInMenu>> GetAllProductInMenuByCategoryIdAndStoreId(string storeId, string categoryId, string menuId, int page, int pageSize)
        {
            return Ok(await repository.Menu.GetAllProductInMenuByCategoryIdAndStoreId(storeId, categoryId, menuId, page, pageSize));
        }

        /// <summary>
        /// Get list products in store and not in a menu (when click add product to menu in store web)
        /// </summary>
        [HttpGet("{menuId}/not-products/filter")]
        public async Task<ActionResult<List<ProductViewInList>>> GetListProductNotInMenuByCategoryIdAndStoreId(string menuId, string storeId, int page, int pageSize)
        {
            return Ok(await repository.Menu.GetListProductNotInMenuByCategoryIdAndStoreId(storeId, menuId, page, pageSize));

        }
        /// <summary>
        /// Add a list product into menu
        /// </summary>
        [HttpPost("{menuId}/products/join")]
        public async Task<ActionResult<ProductsInMenuModel>> AddProductsToMenu(string menuId, ProductsInMenuModel listProduct)
        {
            //try
            //{
            //    return Ok(await menuRepository.AddProductsToMenu(listProduct));
            //}
            //catch
            //{
            //    return BadRequest();
            //}
            return Ok(await repository.Menu.AddProductsToMenu(listProduct));
        }
    }
}
