using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ProductCatalog.Api.Domain.Product;
using ProductCatalog.Tests.DataHelpers;
using Xunit;

namespace ProductCatalog.Tests
{
    public class TrolleyTotalTests
    {
        [Fact]
        public async Task SortEndpointIsConfiguredAndReturnsCorrectJsonResponseForRecommended()
        {
            var httpClient = new WebApplicationFactory<ProductCatalog.Api.Startup>().Server.CreateClient();
            var httpResponseMessage = await httpClient.PostAsync("/trolleyTotal",
                new StringContent(
                    "{'products': [{'name': 'test','price': 100.0}],'specials': [{'quantities': [{'name': 'test','quantity': 2}],'total':150}],'quantities': [{'name': 'test','quantity': 2}]}"));
            
            httpResponseMessage.StatusCode.Should().Be(StatusCodes.Status200OK);
            var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
            readAsStringAsync.Should().Be("150.0");
        }
    }
}