using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Domain.Product;

namespace ProductCatalog.Api.Controllers
{
    [Route("/products")]
    public class ProductController : ControllerBase
    {
        [HttpGet("/sort")]
        public IActionResult Sort([FromQuery] string sortOption)
        {
            return Ok(new Product[] { });
        }
    }
}