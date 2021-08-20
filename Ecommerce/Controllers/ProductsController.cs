using Ecommerce.Model;
using Ecommerce.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private IProductRepo _productRepo;
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;

        public ProductsController(IProductRepo productRepo, IMemoryCache cache, ILogger<ProductsController> logger)
        {
            _productRepo = productRepo;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Products>))]
        public async Task<IActionResult> GetAllProducts()
        {
            _logger.LogInformation("started GetAllProducts() method");
            var cacheKey = "GET_ALL_PRODUCTS";

            if (_cache.TryGetValue(cacheKey, out List<Products> products))
            {
                return Ok(products);
            }

            var myTask = Task.Run(() => _productRepo.GetAllProducts());
            var objList = await myTask;
            _cache.Set(cacheKey, objList);
            _logger.LogInformation("ended GetAllProducts() method");
            return Ok(objList);
        }

    }
}
