using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using OrderEndpoint.Model;
using OrderEndpoint.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderEndpoint.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class Order : Controller
    {
        public Order()
        {
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Product>))]
        public List<ProductDto> GetAllProducts()
        {
            var responseString = ApiCall.GetApi("https://localhost:44332/api/Products");
            var rootobject = new JavaScriptSerializer().Deserialize<List<ProductDto>>(responseString);
            return rootobject;
        }

        [HttpGet("{ProductsId:int}", Name = "GetProduct")]
        [ProducesResponseType(200, Type = typeof(List<Product>))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetOneProducts(int ProductsId)
        {
            var url = "https://localhost:44332/api/Products/" + ProductsId;
            Console.WriteLine(url);
            var responseString = ApiCall.GetApi(url);
            Console.WriteLine(responseString);
            return Ok(responseString);
            //var rootobject = new JavaScriptSerializer().Deserialize<List<ProductDto>>(responseString);
            //return rootobject;
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public List<ProductDto> CreateProducts([FromBody] ProductDto products)
        {
            try
            {
                var responseString = ApiCall.PostApi("https://localhost:44332/api/Products", products.ToString());
                var rootobject = new JavaScriptSerializer().Deserialize<List<ProductDto>>(responseString);
                return rootobject;
            }
            catch (Exception ex)
            {
                return new List<ProductDto>();
            }

        }

    }
}
