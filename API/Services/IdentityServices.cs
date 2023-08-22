using API.Authentication.Basic;
using Microsoft.AspNetCore.Authentication;

namespace API.Services
{
    public static class IdentityServices
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services
                .AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

            return services;
        }
    }
}
