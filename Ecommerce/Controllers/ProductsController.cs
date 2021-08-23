using Ecommerce.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Ecommerce.Model;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private IProductRepo _productRepo;
        private readonly IMemoryCache _cache;
        private readonly ILog logger;
        private readonly IHttpClientFactory _clientFactory;

        public ProductsController(ILog logger,IProductRepo productRepo, IMemoryCache cache, IHttpClientFactory clientFactory)
        {
            _productRepo = productRepo;
            _cache = cache;
            this.logger = logger;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Products>))]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                logger.Information("Information is logged");
                var cacheKey = "GET_ALL_PRODUCTS";

                if (_cache.TryGetValue(cacheKey, out List<Products> products))
                {
                    return Ok(products);
                }

                var myTask = Task.Run(() => _productRepo.GetAllProducts());
                var objList = await myTask;
                _cache.Set(cacheKey, objList);
                logger.Information("Information is logged");
                return Ok(objList);
            }
            catch (Exception ex)
            {
                logger.Error($"Something went wrong: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }



    }
}
