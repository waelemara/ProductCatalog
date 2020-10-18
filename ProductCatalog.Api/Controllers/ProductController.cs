using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Domain.Product;

namespace ProductCatalog.Api.Controllers
{
    [Route("/products")]
    public class ProductController : ControllerBase
    {
        private readonly GetSortedProductQueryHandler _getSortedProductQueryHandler;

        public ProductController(GetSortedProductQueryHandler getSortedProductQueryHandler)
        {
            _getSortedProductQueryHandler = getSortedProductQueryHandler;
        }
        
        [HttpGet("/sort")]
        public async Task<ActionResult<IEnumerable<Product>>> Sort([FromQuery] string sortOption)
        {
            var getSortedProductQuery = new GetSortedProductQuery(sortOption);
            var queryResponse = await _getSortedProductQueryHandler.Handle(getSortedProductQuery);
            return Ok(queryResponse.Products);
        }
    }
}