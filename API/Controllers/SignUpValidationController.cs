using API.Authentication.Basic.Attributes;
using Application.Users;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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
                //Only allow English letter, numbers and underscore
                Regex regex = new Regex(@"^[a-zA-Z0-9_]+$");

                if (!regex.IsMatch(user.DisplayName))
                {
                    return Results.BadRequest(
                        new
                        {
                            version = "1.0.0",
                            status = "400",
                            action = "ValidationError",
                            userMessage = "Only letters, numbers and underscore allowed for Display Name"
                        }
                    );
                }

                var results = await userData.GetUserWithDisplayName(user.DisplayName);

                if (results.DisplayName == null)
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
