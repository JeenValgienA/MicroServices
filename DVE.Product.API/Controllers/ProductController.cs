using DVE.Product.DataEntity;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;

namespace DVE.Product.Requester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly IBus _bus;
        public ProductController(IBus bus) => _bus = bus;


        /// <summary>
        /// API to list all the products
        /// </summary>
        /// <returns>Returns the product list</returns>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var task = _bus.Rpc.RequestAsync<Byte, List<ProductName>>(0);
                var res = await task.ContinueWith(response =>
                {
                    return response;
                });

                return Ok(res.Result);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        /// <summary>
        /// API to read the particular product based on product id.
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns>Returns the product detail</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            try
            {
                var task = _bus.Rpc.RequestAsync<string, ProductDetail>(id);
                var productResponse = await task.ContinueWith(response =>
                {
                    return response;
                });

                if (productResponse != null)
                    return Ok(productResponse.Result);
                else
                    return NotFound();
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}
