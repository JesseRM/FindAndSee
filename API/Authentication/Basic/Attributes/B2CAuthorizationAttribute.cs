using Microsoft.AspNetCore.Authorization;

namespace API.Authentication.Basic.Attributes
{
    public class B2CAuthorizationAttribute : AuthorizeAttribute
    {
        public B2CAuthorizationAttribute()
        {
            AuthenticationSchemes = "Basic";
        }
    }
}
