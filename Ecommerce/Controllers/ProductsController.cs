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
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

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
        private readonly IConfiguration _configuration;
        private readonly string connString;

        public ProductsController(IConfiguration configuration,ILog logger,IProductRepo productRepo, IMemoryCache cache, IHttpClientFactory clientFactory)
        {
            _configuration = configuration;
            _productRepo = productRepo;
            _cache = cache;
            this.logger = logger;
            _clientFactory = clientFactory;
            var host = _configuration["DBHOST"] ?? "localhost";
            var port = _configuration["DBPORT"] ?? "3306";
            var password = _configuration["MYSQL_PASSWORD"] ?? "";
            var userid = _configuration["MYSQL_USER"] ?? "";
            var usersDataBase = _configuration["MYSQL_DATABASE"] ?? "Products";

            connString = $"server={host}; userid={userid};pwd={password};port={port};database={usersDataBase}";
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
                using (var connection = new MySqlConnection(connString))
                {
                    var myTask = Task.Run(() => _productRepo.GetAllProducts());
                    var objList = await myTask;
                    _cache.Set(cacheKey, objList);
                    logger.Information("Information is logged");
                    return Ok(objList);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Something went wrong: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }



    }
}
