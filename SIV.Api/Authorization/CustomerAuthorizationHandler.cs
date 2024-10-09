using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Tokens;

namespace SIV.Api.Authorization
{
    public class CustomerAuthorizationHandler : AuthorizationHandler<SSORequirement>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly AuthDataService authDataService;
        private readonly IAuthenticationSchemeProvider _schemes;

        public CustomerAuthorizationHandler(IAuthenticationSchemeProvider schemes, IHttpContextAccessor accessor, AuthDataService authDataService)
        {
            _accessor = accessor;
            this.authDataService = authDataService;
            _schemes = schemes;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SSORequirement requirement)
        {
            var httpContext = _accessor.HttpContext;

            if (httpContext == null)
            {
                context.Fail();
                return;
            }

            try
            {

                if (requirement.Permissions.IsNullOrEmpty())
                    requirement.Permissions = authDataService.GetHierarchy();

                var requestUrl = httpContext.Request.Path.Value.Trim().ToLower();
                var requestResource = requestUrl.Split("/", StringSplitOptions.RemoveEmptyEntries);

                var defaultAuthenticate = await _schemes.GetDefaultAuthenticateSchemeAsync();
                if (defaultAuthenticate == null)
                {
                    context.Fail();
                    return;
                }

                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                if (result == null || result?.Principal == null)
                {
                    context.Fail();
                    return;
                }

                var identity = result.Principal.Identities.FirstOrDefault();
                httpContext.User = result.Principal;

                // 获取当前用户ID
                var userIdStr = (from item in httpContext.User.Claims
                                 where item.Type == requirement.ClaimType
                                 select item.Value).FirstOrDefault();

                if (userIdStr.IsNullOrEmpty())
                {
                    context.Fail();
                    return;
                }

                if (!int.TryParse(userIdStr, out var userId))
                {
                    context.Fail();
                    return;
                }

                var permission = requirement.Permissions[userId]; 

                if (!permission.ApiPaths.Contains(requestUrl))
                {
                    context.Fail();
                    return;
                }

                context.Succeed(requirement);
            }
            catch (Exception)
            {
                context.Fail();
                return;
            }
        }
    }
}
