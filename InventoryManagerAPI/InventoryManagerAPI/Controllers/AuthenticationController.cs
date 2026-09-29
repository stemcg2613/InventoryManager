using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace InventoryManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpGet]
        public IActionResult Authenticate()
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Headers.Authorization))
            {
                return Unauthorized();
            }

            var authHeader =
                HttpContext.Request.Headers.Authorization.ToString();

            if (authHeader.StartsWith("Basic "))
            {
                try
                {
                    var encodedCredentials = authHeader.Substring(6);

                    var credentials = Encoding.UTF8.GetString(
                        Convert.FromBase64String(encodedCredentials));

                    var parts = credentials.Split(':');

                    if (parts.Length == 2 &&
                        parts[0].ToLower() == "instructor" &&
                        parts[1] == "Password")
                    {
                        return Ok("Success");
                    }
                }
                catch
                {
                    return Unauthorized();
                }
            }

            return Unauthorized();
        }
    }
}