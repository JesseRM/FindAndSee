using API.Authentication.Basic.Attributes;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Persistence.Data;
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
            using var reader = new StreamReader(HttpContext.Request.Body);

            // You shouldn't need this line anymore.
            // reader.BaseStream.Seek(0, SeekOrigin.Begin);

            // You now have the body string raw
            var body = await reader.ReadToEndAsync();

            // As well as a bound model

            Console.WriteLine(body);
            try
            {
                if (user.NewUser == true)
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
        public async Task<IResult> Validate(User user, IUserData userData)
        {
            try
            {
                //Only allow English letters, numbers and underscore
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
