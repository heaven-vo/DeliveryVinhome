using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.IRepositories;

namespace DeliveryVHGP_WebApi.Controllers
{
    [Route("api/v1/collection")]
    [ApiController]
    public class CollectionsController : ControllerBase
    {
        private readonly ICollectionRepository _collectionRepository;

        public CollectionsController(ICollectionRepository collectionRepository)
        {
            _collectionRepository = collectionRepository;
        }

        /// <summary>
        /// Get list all collection with pagination
        /// </summary>
        //GET: api/v1/collection?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await _collectionRepository.GetAll(pageIndex, pageSize));
        }

        /// <summary>
        /// Get a collection by id
        /// </summary>
        //GET: api/v1/collection/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            var collection = await _collectionRepository.GetById(id);
            if (collection == null)
                return NotFound();
            return Ok(collection);
        }
    }
}
