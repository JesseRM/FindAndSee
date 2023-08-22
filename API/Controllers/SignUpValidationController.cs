using API.Authentication.Basic.Attributes;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistence.Data;
using System.Diagnostics;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignUpValidationController : ControllerBase
    {
        [HttpPost, B2CAuthorization]
        public async Task<IResult> Validate(User user, IUserData userData)
        {
            try
            {
                var results = await userData.GetUser(user.DisplayName);

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
            catch (Exception)
            {
                return Results.Ok(
                    new
                    {
                        version = "1.0.0",
                        action = "ShowBlockPage",
                        userMessage = "There was an error processing your request, try again later"
                    }
                );
            }
        }
    }
}
