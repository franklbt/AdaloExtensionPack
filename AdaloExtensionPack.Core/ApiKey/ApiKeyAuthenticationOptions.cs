using Microsoft.AspNetCore.Authentication;

namespace AdaloExtensionPack.Core.ApiKey
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "API Key";
        public string Scheme => DefaultScheme;
        public readonly string AuthenticationType = DefaultScheme;
        public string ApiKey { get; set; }
    }
}
