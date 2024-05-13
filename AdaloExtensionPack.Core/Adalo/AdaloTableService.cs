using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace AdaloExtensionPack.Core.Adalo
{
    public class AdaloTableService<T> : IAdaloTableService<T>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Guid _appId;
        private readonly string _tableId;
        private readonly HttpClient _client;

        public AdaloTableService(IHttpClientFactory httpClientFactory, Guid appId, string token, string tableId)
        {
            _httpClientFactory = httpClientFactory;
            _appId = appId;
            _tableId = tableId;
            _client = GetHttpClient(token);
        }

        public async Task<List<T>> GetAllAsync(
            params (Expression<Func<T, object>> Predicate, object Value)[] predicates)
        {
            var url = GetUrl();
            var filters = predicates is { Length: > 0 }
                ? predicates
                    .Where(x => x.Predicate.Body is MemberExpression)
                    .Select(x => (((MemberExpression)x.Predicate.Body).Member.Name, x.Value))
                    .ToDictionary(x => x.Name, x => x.Value.ToString())
                : new Dictionary<string, string>();
            
            var finalUrl = QueryHelpers.AddQueryString(url, filters);
            
            return await _client.GetFromJsonAsync<List<T>>(finalUrl);
        }

        public async Task<T> PostAsync(T payload)
        {
            var url = GetUrl();
            var response = await _client.PostAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T> GetAsync(int recordId)
        {
            var url = GetUrl(recordId);
            return await _client.GetFromJsonAsync<T>(url);
        }

        public async Task DeleteAsync(int recordId)
        {
            var url = GetUrl(recordId);
            var response = await _client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }

        public async Task<T> PutAsync(int recordId, T payload)
        {
            var url = GetUrl(recordId);
            var response = await _client.PutAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        private HttpClient GetHttpClient(string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + token);
            return client;
        }

        private string GetUrl(int? recordId = null)
        {
            return
                $"https://api.adalo.com/apps/{_appId}/collections/{_tableId}{(recordId != null ? "/" + recordId : "")}";
        }
    }
}
