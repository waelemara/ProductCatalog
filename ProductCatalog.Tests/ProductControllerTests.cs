using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ProductCatalog.Api.Controllers;
using ProductCatalog.Api.Domain.Product;
using Xunit;

namespace ProductCatalog.Tests
{
    public class ProductSortController
    {
        
        [Fact]
        public async Task FindUserReturnsCorrectDetails_ForWael()
        {
            var result = await new ProductController(new GetSortedProductQueryHandler(StubProductsHttpClient.WithEmptyProducts())).Sort("Low");
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Product>>>(result);
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            okObjectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            
            var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okObjectResult.Value);
            products.Should().BeEmpty();
        }

        [Fact]
        public async Task UserEndpointIsConfiguredAndReturnsCorrectJsonResponse()
        {
            var httpClient = new WebApplicationFactory<ProductCatalog.Api.Startup>().Server.CreateClient();
            var httpResponseMessage = await httpClient.GetAsync("/sort?sortOption=High");
            httpResponseMessage.StatusCode.Should().Be(StatusCodes.Status200OK);
            var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<Product[]>(readAsStringAsync);
            products.Should().Equal(ListOfProduct.SortedProductsFormHighToLow);
        }
    }
}