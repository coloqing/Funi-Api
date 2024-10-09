using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Encodings.Web;

namespace SIV.Api.Authorization
{
    public class CustomerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CustomerAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
           ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            await Response.WriteAsync(JsonConvert.SerializeObject(new ChallengeResponse
            {
                Code = 401,
                Message = "Unauthorized",
            }));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status403Forbidden;
            await Response.WriteAsync(JsonConvert.SerializeObject(new ChallengeResponse
            {
                Code = 403,
                Message = "Forbidden",
            }));
        }
    }

    class ChallengeResponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }
    }
}
