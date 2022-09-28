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
    [Route("api/v1/store")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IMenuRepository _menuRepository;

        public StoresController(IStoreRepository storeRepository, IMenuRepository menuRepository)
        {
            _storeRepository = storeRepository;
            _menuRepository = menuRepository;
        }

        /// <summary>
        /// Get list all store with pagination
        /// </summary>
        //GET: api/v1/store?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await _storeRepository.GetAll(pageIndex, pageSize));
        }
    }
}
