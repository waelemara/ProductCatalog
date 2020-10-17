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
        public void FindUserReturnsCorrectDetails_ForWael()
        {
            var result = new ProductController().Sort("Low");
            var objectResult = Assert.IsType<OkObjectResult>(result);
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            var products = Assert.IsType<Product[]>(objectResult.Value);
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
            products.Should().BeEmpty();
        }
    }
}