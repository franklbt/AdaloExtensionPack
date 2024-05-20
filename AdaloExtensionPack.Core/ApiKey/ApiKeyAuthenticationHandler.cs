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
    internal class ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : AuthenticationHandler<ApiKeyAuthenticationOptions>(options, logger, encoder)
    {
        private readonly IOptionsMonitor<ApiKeyAuthenticationOptions> _options = options;
        private const string ProblemDetailsContentType = "application/problem+json";

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(Options.ApiKeyHeaderName, out var apiKeyHeaderValues))
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
                new(ClaimTypes.Name, "System")
            };

            var identity = new ClaimsIdentity(claims, Options.Scheme);
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
