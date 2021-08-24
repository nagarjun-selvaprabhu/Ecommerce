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
                logger.Information("Inside GetAllProducts method");
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
                    logger.Information("GetAllProducts method Exited");
                    return Ok(objList);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Something went wrong: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{ProductsId:int}", Name = "GetProduct")]
        [ProducesResponseType(200, Type = typeof(Products))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetProduct(int ProductsId)
        {
            try
            {
                var obj = _productRepo.GetProduct(ProductsId);
                if (obj == null)
                {
                    return NotFound();
                }
                return Ok(obj);
            }
            catch (Exception ex)
            {
                logger.Error($"Something went wrong: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Products))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProducts([FromBody] Products products)
        {
            try
            {
                if (products == null)
                {
                    return BadRequest();
                }
                if (_productRepo.ProductExists(products.ProductName))
                {
                    ModelState.AddModelError("", "National Park Exists!");
                    return StatusCode(404, ModelState);
                }
                var flag = _productRepo.CreateProduct(products);
                if (flag)
                {
                    return Ok("successfully created");
                }
                else
                {
                    return StatusCode(404, ModelState);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Something went wrong: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch("{ProductsId:int}", Name = "UpdateProductsk")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateProductsk(int ProductsId, [FromBody] Products products)
        {
            try
            {
                if (products == null || ProductsId != products.Id)
                {
                    return BadRequest(ModelState);
                }
                var flag = _productRepo.UpdateProduct(products);
                if (flag)
                {
                    return Ok("successfully updated");
                }
                else
                {
                    return StatusCode(404, ModelState);
                }
            }
            catch(Exception e)
            {
                logger.Error($"Something went wrong: {e}");
                return StatusCode(500, "Internal server error");
            }

        }


    }
}
