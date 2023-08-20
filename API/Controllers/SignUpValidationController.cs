using Microsoft.AspNetCore.Mvc;
using Persistence.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignUpValidationController : ControllerBase
    {
        [HttpPost("validate")]
        public async Task<IResult> Validate(string displayName, IUserData userData)
        {
            try
            {
                var results = await userData.GetUser(displayName);

                if (results == null)
                {
                    return Results.Ok(new { version = "1.0.0", action = "continue" });
                }
                else
                {
                    return Results.BadRequest(
                        new
                        {
                            version = "1.0.0",
                            status = "400",
                            action = "ValidationError",
                            userMessage = "Display Name already exists, try another"
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                return Results.BadRequest(
                    new
                    {
                        version = "1.0.0",
                        status = "400",
                        action = "ValidationError",
                        userMessage = ex.Message
                    }
                );
            }
        }
    }
}
