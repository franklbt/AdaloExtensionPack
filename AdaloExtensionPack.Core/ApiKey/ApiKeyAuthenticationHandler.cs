using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdaloExtensionPack.Core.ApiKey
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IOptionsMonitor<ApiKeyAuthenticationOptions> _options;
        private const string ProblemDetailsContentType = "application/problem+json";
        private const string ApiKeyHeaderName = "X-Api-Key";

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _options = options;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var isValidApiKey = _options.Get(ApiKeyAuthenticationOptions.DefaultScheme).ApiKey.Equals(providedApiKey);

            if (!isValidApiKey) return Task.FromResult(AuthenticateResult.Fail("Invalid API Key provided."));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "System")
            };

            var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
            var identities = new List<ClaimsIdentity> {identity};
            var principal = new ClaimsPrincipal(identities);
            var ticket = new AuthenticationTicket(principal, Options.Scheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            Response.ContentType = ProblemDetailsContentType;
            return Task.CompletedTask;
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            Response.ContentType = ProblemDetailsContentType;
            return Task.CompletedTask;
        }
    }
}