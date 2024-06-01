using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace AdaloExtensionPack.Core.Tables.Services
{
    public class AdaloTableService<T> : IAdaloTableService<T> where T : AdaloEntity
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

        /// <summary>
        /// Get all records for a table
        /// </summary>
        /// <param name="predicate">A filter on one field sent to the server. Must be a top-level simple field (text, number, bool, date & time - not array)</param>
        /// <returns>A list of records</returns>
        public async Task<List<T>> GetAllAsync(
            (Expression<Func<T, object>> Predicate, object Value)? predicate = null)
        {
            var url = GetTableUrl(_appId, _tableId);
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<GetAllResponse>();
            return result?.Records ?? [];
        }

        private static string GetMemberName(MemberExpression m)
        {
            return ((JsonPropertyNameAttribute)m.Member
                       .GetCustomAttribute(typeof(JsonPropertyNameAttribute), true))?.Name
                   ?? m.Member.Name;
        }

        /// <summary>
        /// Create a new record on adalo database
        /// </summary>
        /// <param name="payload">The entity to add</param>
        /// <returns>The added entity</returns>
        public async Task<T> PostAsync(T payload)
        {
            var url = GetTableUrl(_appId, _tableId);
            var response = await _client.PostAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        /// <summary>
        /// Get a single record
        /// </summary>
        /// <param name="recordId">Id of the record to fetch</param>
        /// <returns>The record to get, or null if not found</returns>
        public async Task<T> GetAsync(int recordId)
        {
            var url = GetTableUrl(_appId, _tableId, recordId);
            var response = await _client.GetAsync(url);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }

        /// <summary>
        /// Delete a record on the database
        /// </summary>
        /// <param name="recordId">Id of the deleted record</param>
        public async Task DeleteAsync(int recordId)
        {
            var url = GetTableUrl(_appId, _tableId, recordId);
            var response = await _client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Update a record on Adalo database
        /// </summary>
        /// <param name="recordId">Id of the record</param>
        /// <param name="payload">Entity containing new properties values</param>
        /// <returns>The updated record</returns>
        public async Task<T> PutAsync(int recordId, T payload)
        {
            var url = GetTableUrl(_appId, _tableId, recordId);
            var response = await _client.PutAsJsonAsync(url, payload, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            response.EnsureSuccessStatusCode();
            return payload;
        }

        /// <summary>
        /// Update a record on Adalo Database using an update action
        /// </summary>
        /// <param name="recordId">Id of the record</param>
        /// <param name="update">Action containing new properties update</param>
        /// <returns>The updated record</returns>
        public async Task<T> PutAsync(int recordId, Action<T> update)
        {
            var record = await GetAsync(recordId);
            if (record is null)
            {
                return null;
            }

            update(record);
            return await PutAsync(recordId, record);
        }

        private HttpClient GetHttpClient(string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + token);
            return client;
        }


        public static string GetTableUrl(Guid appId, string tableId, int? recordId = null,
            (Expression<Func<T, object>> Predicate, object Value)? predicate = null)
        {
            var url =
                $"https://api.adalo.com/v0/apps/{appId}/collections/{tableId}{(recordId != null ? "/" + recordId : "")}";

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

            return url;
        }

        private class GetAllResponse
        {
            public List<T> Records { get; init; }
            public int Offset { get; init; }
        }
    }
}
