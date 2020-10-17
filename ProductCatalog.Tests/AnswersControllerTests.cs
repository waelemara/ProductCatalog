using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductCatalog.Api.Controllers;
using Xunit;

namespace ProductCatalog.Tests
{
    public class AnswersControllerTests
    {
        [Fact]
        public void FindUserReturnsCorrectDetails_ForWael()
        {
            var result = new UserController().FindUser();
            var objectResult = Assert.IsType<OkObjectResult>(result);
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            var userResponseModel = Assert.IsType<UserResponseModel>(objectResult.Value);
            userResponseModel.Name.Should().Be("Wael Emara");
            userResponseModel.Token.Should().Be("25a4f06f-8fd5-49b3-a711-c013c156f8c8");
        }

        [Fact]
        public async Task UserEndpointIsConfiguredAndReturnsCorrectJsonResponse()
        {
            var httpClient = new WebApplicationFactory<ProductCatalog.Api.Startup>().Server.CreateClient();
            var httpResponseMessage = await httpClient.PostAsync("/user", null);
            httpResponseMessage.StatusCode.Should().Be(StatusCodes.Status200OK);
            var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
            var userResponseModel = JsonConvert.DeserializeObject<UserResponseModel>(readAsStringAsync);
            userResponseModel.Name.Should().Be("Wael Emara");
            userResponseModel.Token.Should().Be("25a4f06f-8fd5-49b3-a711-c013c156f8c8");
        }
    }
}