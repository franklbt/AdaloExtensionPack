using Microsoft.AspNetCore.Authentication;

namespace AdaloExtensionPack.Core.ApiKey
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "API Key";
        public string Scheme { get; set; } = DefaultScheme;
        public string ApiKey { get; set; }
        public string ApiKeyHeaderName { get; set; } = "X-Api-Key";
    }
}
