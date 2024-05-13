using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace AdaloExtensionPack.Core.Adalo
{
    public class AdaloTableService<T> : IAdaloTableService<T>
    {
        private const string JsonPropertyNameAttributeName = "JsonPropertyNameAttribute";
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

        /// <summary>
        /// Get all records for a table
        /// </summary>
        /// <param name="predicate">A filter on one field sent to the server. Must be a simple field (text, number, bool, date & time - not array)</param>
        /// <returns>A list of records</returns>
        public async Task<List<T>> GetAllAsync(
            (Expression<Func<T, object>> Predicate, object Value)? predicate = null)
        {
            var url = GetUrl();
            var filter = predicate is { Predicate.Body: MemberExpression m }
                ? (Name: GetMemberName(m), predicate.Value.Value)
                : default;

            if (filter != default)
            {
                var param = new Dictionary<string, string>
                {
                    ["filterKey"] = filter.Name,
                    ["filterValue"] = filter.Value.ToString()
                };

                url = QueryHelpers.AddQueryString(url, param);
            }

            return await _client.GetFromJsonAsync<List<T>>(url);
        }

        private static string GetMemberName(MemberExpression m)
        {
            return ((JsonPropertyNameAttribute)m.Member
                       .GetCustomAttribute(typeof(JsonPropertyNameAttribute), true))?.Name
                   ?? m.Member.Name;
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
