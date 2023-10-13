using API.Authentication.Basic.Attributes;
using Application.Users;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpPost("create"), B2CAuthorization]
        public async Task<IResult> CreateUser(UserCreate user, IUserData userData)
        {
            var registeredUser = await userData.GetUserWithDisplayName(user.DisplayName);

            try
            {
                if (registeredUser == null)
                {
                    await userData.CreateUser(user);
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

            return Results.Ok(new { version = "1.0.0", action = "continue" });
        }

        [HttpPost("SignUpValidation"), B2CAuthorization]
        public async Task<IResult> Validate(UserCreate user, IUserData userData)
        {
            try
            {
                user.DisplayName ??= "";

                //Only allow English letters, numbers and underscore
                Regex regex = new Regex(@"^(?=.*[a-zA-Z].*)([a-zA-Z0-9_]+)$");

                if (!regex.IsMatch(user.DisplayName))
                {
                    return Results.BadRequest(
                        new
                        {
                            version = "1.0.0",
                            status = "400",
                            action = "ValidationError",
                            userMessage = "Display Name must be alphanumeric with at least one letter (underscore allowed)"
                        }
                    );
                }

                var results = await userData.GetUserWithDisplayName(user.DisplayName);

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
