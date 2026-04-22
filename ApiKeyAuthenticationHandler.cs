using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace APIKeys_MinimalAPIs
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IOptionsMonitor<ApiOptions> _apiOptions;
        public const string SchemeName = "ApiKey";

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            IOptionsMonitor<ApiOptions> apiOptions,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder)
            : base(options, loggerFactory, urlEncoder)
        {
            _apiOptions = apiOptions;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("api-key", out var apiHeaderValue))
            {
                return Task.FromResult(AuthenticateResult.Fail("API key missing"));
            }

            if (apiHeaderValue != _apiOptions.CurrentValue.Key)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid API key"));
            }

            var identity = new ClaimsIdentity(
                [new Claim(ClaimTypes.AuthenticationMethod, SchemeName)], SchemeName);

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

}
