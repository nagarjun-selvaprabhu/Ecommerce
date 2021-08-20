using Ecommerce.Model;
using Ecommerce.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private IProductRepo _productRepo;

        public ProductsController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Products>))]
        public IActionResult GetAllProducts()
        {
            var objList = _productRepo.GetAllProducts();
            return Ok(objList);
        }

    }
}
