using Ecommerce.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
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

        public ProductsController(ILog logger,IProductRepo productRepo, IMemoryCache cache)
        {
            _productRepo = productRepo;
            _cache = cache;
            this.logger = logger;
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
                    var myTask = Task.Run(() => _productRepo.GetAllProducts());
                    var objList = await myTask;
                    _cache.Set(cacheKey, objList);
                    logger.Information("GetAllProducts method Exited");
                    return Ok(objList);
            }
            catch (Exception ex)
            {
                logger.Error($"Something went wrong: {ex}");
                return StatusCode(500, "Something Went Wrong!");
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
                logger.Information("Inside GetProduct method");
                var obj = _productRepo.GetProduct(ProductsId);
                if (obj == null)
                {
                    return NotFound();
                }
                logger.Information("GetProduct method Exited");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                logger.Error($"Something went wrong: {ex}");
                return StatusCode(500, "Something Went Wrong!");
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
                logger.Information("Inside CreateProducts method");
                if (products == null)
                {
                    return BadRequest();
                }
                if (_productRepo.ProductExists(products.ProductName))
                {
                    ModelState.AddModelError("", "Product Already Exists!");
                    return StatusCode(404, ModelState);
                }
                var flag = _productRepo.CreateProduct(products);
                if (flag)
                {
                    logger.Information("CreateProducts method Exited");
                    return Ok("successfully created");
                }
                else
                {
                    logger.Information("CreateProducts method Exited");
                    return StatusCode(404, ModelState);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Something went wrong: {ex}");
                return StatusCode(500, "Something Went Wrong!");
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
                return StatusCode(500, "Something Went Wrong!");
            }

        }


    }
}
