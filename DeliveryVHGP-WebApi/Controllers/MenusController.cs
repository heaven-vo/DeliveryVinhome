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
    [Route("api/menus")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMenuRepository menuRepository;

        public MenusController(IMenuRepository menuRepository)
        {
            this.menuRepository = menuRepository;
        }

        // GET: api/Menus
        [HttpGet]
        public async Task<ActionResult<List<MenuView>>> getMenus()
        {
            return Ok(await menuRepository.getListMenuNow());
        }

        [HttpGet("{id}/products")]
        public async Task<ActionResult<List<ProductViewInList>>> getAllProductInMenu(string id, int page, int pageSize)
        {
            return Ok(await menuRepository.getListProductInMenu(id, page, pageSize));
        }
    }
}
