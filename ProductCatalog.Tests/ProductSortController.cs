using Xunit;
using System.Net;
using Newtonsoft.Json;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using ProductCatalog.Tests.Stubs;
using ProductCatalog.Api.Controllers;
using ProductCatalog.Tests.DataHelpers;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductCatalog.Api.Domain.Product;

namespace ProductCatalog.Tests
{
    public class ProductSortController
    {
        
        [Fact]
        public async Task SortUserReturnsCorrectDetails()
        {
            var result = await new ProductController(
                new GetSortedProductQueryHandler(StubProductsHttpClient.WithEmptyProducts(),
                    new RecommendationsService(StubShopperHistoryHttpClient.WithNoHistory())))
                .Sort("Low");
            
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Product>>>(result);
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            okObjectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            
            var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okObjectResult.Value);
            products.Should().BeEmpty();
        }

        [Fact]
        public async Task SortEndpointIsConfiguredAndReturnsCorrectJsonResponse()
        {
            var httpClient = new WebApplicationFactory<ProductCatalog.Api.Startup>().Server.CreateClient();
            var httpResponseMessage = await httpClient.GetAsync("/sort?sortOption=High");
            httpResponseMessage.StatusCode.Should().Be(StatusCodes.Status200OK);
            var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<Product[]>(readAsStringAsync);
            products.Should().Equal(ListOfProduct.SortedProductsFormHighToLow);
        }
        
        [Fact]
        public async Task SortEndpointIsConfiguredAndReturnsCorrectJsonResponseForRecommended()
        {
            var httpClient = new WebApplicationFactory<ProductCatalog.Api.Startup>().Server.CreateClient();
            var httpResponseMessage = await httpClient.GetAsync("/sort?sortOption=Recommended");
            httpResponseMessage.StatusCode.Should().Be(StatusCodes.Status200OK);
            var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<Product[]>(readAsStringAsync);
            products.Should().Equal(ListOfProduct.SortedBasedOnRecommended);
        }
    }
}