using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace API.Authentication.Basic
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _config;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration config
        )
            : base(options, logger, encoder, clock)
        {
            _config = config;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization key"));
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();

            if (!authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(
                    AuthenticateResult.Fail("Authorization header does not start with 'Basic '")
                );
            }

            var authBase64Decoded = Encoding.UTF8.GetString(
                Convert.FromBase64String(
                    authorizationHeader.Replace("Basic ", "", StringComparison.OrdinalIgnoreCase)
                )
            );

            var authSplit = authBase64Decoded.Split(new[] { ':' }, 2);

            if (authSplit.Length != 2)
            {
                return Task.FromResult(
                    AuthenticateResult.Fail("Invalid Authorization header format")
                );
            }

            string username = authSplit[0];
            string password = authSplit[1];

            if (
                username != _config["ASPNETCORE_B2C_USERNAME"]
                || password != _config["ASPNETCORE_B2C_PASSWORD"]
            )
            {
                return Task.FromResult(
                    AuthenticateResult.Fail("Authentication credentials are incorrect")
                );
            }

            var client = new BasicAuthenticationClient
            {
                AuthenticationType = "Basic",
                IsAuthenticated = true,
                Name = username,
            };

            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(client, new[] { new Claim(ClaimTypes.Name, username) })
            );

            return Task.FromResult(
                AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name))
            );
        }
    }
}
