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
using AutoMapper;
using Ecommerce.Model.Dtos;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private IProductRepo _productRepo;
        private readonly IMemoryCache _cache;
        private readonly ILog logger;
        private readonly IMapper _mapper;

        public ProductsController(ILog logger,IProductRepo productRepo, IMemoryCache cache, IMapper mapper)
        {
            _productRepo = productRepo;
            _cache = cache;
            this.logger = logger;
            _mapper = mapper;
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
                    var objDto = new List<ProductDto>();
                    foreach (var obj in objList)
                    {
                        objDto.Add(_mapper.Map<ProductDto>(obj));
                    }
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
                var objDto = _mapper.Map<ProductDto>(obj);
                if (obj == null)
                {
                    return NotFound();
                }
                logger.Information("GetProduct method Exited");
                return Ok(objDto);
            }
            catch (Exception ex)
            {
                logger.Error($"Something went wrong: {ex}");
                return StatusCode(500, "Something Went Wrong!");
            }
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProducts([FromBody] ProductDto products)
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
                var ProductObj = _mapper.Map<Products>(products);
                if (!_productRepo.CreateProduct(ProductObj))
                {
                    ModelState.AddModelError("", $"Something went wrong when saving the record {ProductObj.ProductName}");
                    return StatusCode(500, ModelState);
                }
                return CreatedAtRoute("GetProduct", new { ProductsId = ProductObj.Id }, ProductObj);
            }
            catch (Exception ex)
            {
                logger.Error($"Something went wrong: {ex}");
                return StatusCode(500, "Something Went Wrong!");
            }
        }

        [HttpPatch("{ProductsId:int}", Name = "UpdateProducts")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateProductsk(int ProductsId, [FromBody] ProductDto products)
        {
            try
            {
                if (products == null || ProductsId != products.Id)
                {
                    return BadRequest(ModelState);
                }
                var ProductObj = _mapper.Map<Products>(products);
                if (!_productRepo.UpdateProduct(ProductObj))
                {
                    ModelState.AddModelError("", $"Something went wrong when updating the record {ProductObj.ProductName}");
                    return StatusCode(500, ModelState);
                }

                return NoContent();
            }
            catch(Exception e)
            {
                logger.Error($"Something went wrong: {e}");
                return StatusCode(500, "Something Went Wrong!");
            }

        }



    }
}
