using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProductCatalog.Api.Controllers
{
    [Route("user")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult FindUser()
        {
            return Ok(UserResponseModel.CreateForWael());
        }
    }

    public class UserResponseModel
    {
        public static UserResponseModel CreateForWael()
        {
            return new UserResponseModel
            {
                Name = "Wael Emara",
                Token = "25a4f06f-8fd5-49b3-a711-c013c156f8c8"
            };
        }

        [JsonProperty]

        public string Token { get; private set; }

        [JsonProperty]

        public string Name { get; private set; }
    }
}