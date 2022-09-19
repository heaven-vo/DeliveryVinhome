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
        public async Task<ActionResult<List<MenuView>>> GetMenus()
        {
            return Ok(await menuRepository.GetListMenuNow());
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<MenuView>>> GetMenuByModeId(string modeId)
        {
            return Ok(await menuRepository.GetListMenuByMode(modeId));
        }

        [HttpGet("{id}/products")]
        public async Task<ActionResult<List<ProductViewInList>>> GetAllProductInMenu(string id, int page, int pageSize)
        {
            return Ok(await menuRepository.GetListProductInMenu(id, page, pageSize));
        }
    }
}
