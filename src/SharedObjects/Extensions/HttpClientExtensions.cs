using SharedObjects.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SharedObjects.Extensions
{
    public static class HttpClientExtensions
    {
        public static int BatchSize => 1000;

        /// <summary>
        /// Create an entity of Type T on the API
        /// </summary>
        /// <typeparam name="T">Data type being created</typeparam>
        /// <param name="client">HttpClient instance</param>
        /// <param name="query">Path to API call</param>
        /// <param name="entity">Entity to be created</param>
        /// <returns>The entity with it's populated key after creation</returns>
        public static async ValueTask<T> AddAsync<T>(this HttpClient client, string query, T entity)
        {
            Result<T> validationResults = entity.Validate();

            if (!validationResults.Success)
                throw new ValidationException(validationResults.Message);

            HttpResponseMessage response = await client.PostAsync(query, new StringContent(entity.ToJsonForOdata(), Encoding.UTF8, "application/json"));

            if((int)response.StatusCode > 399)
                throw new Exception($"A HTTP POST to {query} returned HTTP Status: {response.StatusCode}.\n {await response.Content.ReadAsStringAsync()}");

            return await response.Content.ReadAsAsync<T>();
        }

        public static async ValueTask<string> AddStringAsync(this HttpClient client, string query, string data)
        {
            HttpResponseMessage response = await client.PostAsync(query, new StringContent(data.ToString(), Encoding.UTF8, "text/plain"));

            if ((int)response.StatusCode > 399)
                throw new Exception($"A HTTP POST to {query} returned HTTP Status: {response.StatusCode}.\n {await response.Content.ReadAsStringAsync()}");

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Update And Entity of Type T on the API
        /// </summary>
        /// <typeparam name="T">Data type being updated</typeparam>
        /// <param name="client">HttpClient instance</param>
        /// <param name="query">Path to API call</param>
        /// <param name="entity">Entity to be created</param>
        /// <returns>The entity with it's populated key after update</returns>
        public static async ValueTask<T> UpdateAsync<T>(this HttpClient client, string query, T entity)
        {
            Result<T> validationResults = entity.Validate();

            if (!validationResults.Success)
                throw new ValidationException(validationResults.Message);

            HttpResponseMessage response = await client.PutAsync(query, new StringContent(entity.ToJsonForOdata(), Encoding.UTF8, "application/json"));

            if ((int)response.StatusCode > 399)
                throw new Exception($"A HTTP PUT to {query} returned HTTP Status: {response.StatusCode}.\n {await response.Content.ReadAsStringAsync()}");

            return await response.Content.ReadAsAsync<T>();
        }

        /// <summary>
        /// Delete and Entity atthe given URL on the API
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="client">HttpClient instance</param>
        /// <param name="query">Path to API call</param>
        /// <returns>ValueTask that does the work</returns>
        public static async ValueTask DeleteAsync(this HttpClient client, string query)
        {
            var response = await client.DeleteAsync(query);

            if ((int)response.StatusCode > 399)
                throw new Exception($"A HTTP DELETE to {query} returned HTTP Status: {response.StatusCode}.\n {await response.Content.ReadAsStringAsync()}");
        }


        /// <summary>
        /// Update And Entity of Type T on the API Endpoint
        /// </summary>
        /// <typeparam name="T">Data type being updated</typeparam>
        /// <param name="client">HttpClient instance</param>
        /// <param name="query">Path to API call</param>
        /// <param name="entity">Entity to be merged</param>
        /// <returns>The entity with it's populated key after update</returns>
        public static async ValueTask<T> MergeAsync<T>(this HttpClient client, string query, object partialEntity)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), query)
            {
                Content = new StringContent(partialEntity.ToJsonForOdata(), Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request);

            if ((int)response.StatusCode > 399)
                throw new Exception($"A HTTP POST to {query} returned HTTP Status: {response.StatusCode}.\n {await response.Content.ReadAsStringAsync()}");

            return await response.Content.ReadAsAsync<T>();
        }

        /// <summary>
        /// Deletes the given set from the API
        /// </summary>
        /// <typeparam name="TKey">Type of the keys that we are sending</typeparam>
        /// <param name="client">HttpClient instance</param>
        /// <param name="query">Path to API call</param>
        /// <param name="data">Entity keys for the entities to be posted</param>
        /// <returns>Result for each key given showing what happened to each entity on the API</returns>
        public static async ValueTask<IEnumerable<Result<T>>> PostAllAsync<T>(this HttpClient client, string query, IEnumerable<T> data)
        {
            Result<T>[] validationResults = data.Select(i => i.Validate()).ToArray();
            IEnumerable<T> validItems = validationResults.Where(r => r.Success).Select(r => r.Item);
            HttpResponseMessage response = await client.PostAsync(query, new StringContent($"{{ \"value\": {validItems.ToJsonForOdata()} }}", Encoding.UTF8, "application/json"));
            _ = response.EnsureSuccessStatusCode();
            List<Result<T>> results = (await response.Content.ReadAsAsync<ODataCollection<Result<T>>>()).Value.ToList();
            results.AddRange(validationResults.Where(r => !r.Success));
            return results;
        }

        /// <summary>
        /// Fetches a collection of data items of type T from the API
        /// </summary>
        /// <typeparam name="TResult">Expected result type</typeparam>
        /// <param name="client">HttpClient instance</param>
        /// <param name="query">Path to API call</param>
        /// <returns>ValueTask that resolves our Result set as an ODataCollection&gt;T&lt;</returns>
        public static async ValueTask<IEnumerable<T>> GetODataCollection<T>(this HttpClient client, string query)
        {
            List<T> results = new();
            int page = 0;
            string fullQuery = query + (query.Contains('?') ? $"&$skip={page * BatchSize}&$top={BatchSize}" : $"?$skip={page * BatchSize}&$top={BatchSize}");

            ODataCollection<T> batch = await client.GetAsync<ODataCollection<T>>(fullQuery);

            while (batch.Value.Any())
            {
                results.AddRange(batch.Value);
                page++;
                fullQuery = query + (query.Contains('?') ? $"&$skip={page * BatchSize}&$top={BatchSize}" : $"?$skip={page * BatchSize}&$top={BatchSize}");
                batch = await client.GetAsync<ODataCollection<T>>(fullQuery);
            }

            return results;
        }

        /// <summary>
        /// Adds authorization information to the client by making an auth call with the given credentials
        /// </summary>
        /// <param name="client">The HttpClient to attach the authorization information to</param>
        /// <param name="user">The username to use for authentication</param>
        /// <param name="pass">The password to use for authentication</param>
        /// <returns>An authenticated HttpClient</returns>
        public static async ValueTask Authenticate(this HttpClient client, string user, string pass)
        {
            var auth = new { user, pass };
            HttpResponseMessage response = await client.PostAsync("Api/Account/Login", new StringContent(auth.ToJsonForOdata(), Encoding.UTF8, "application/json"));
            if ((int)response.StatusCode == 500 || (int)response.StatusCode == 400)
            {
                var h = await response.Content.ReadAsStringAsync();
                throw new Exception(h);
            }

            _ = response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Adds the given collection of header values to an instance of a http client
        /// </summary>
        /// <param name="client">HttpClient instance</param>
        /// <param name="headers">the header values to add</param>
        /// <returns>HttpClient with the given header values</returns>
        public static HttpClient AddHeaders(this HttpClient client, NameValueCollection headers)
        {
            foreach (object key in headers.Keys)
                client.DefaultRequestHeaders.Add(key.ToString(), headers.Get(key.ToString())); 

            return client;
        }

        /// <summary>
        /// Tells the HttpClient instance to use the given basic auth info in future requests (from credentials)
        /// </summary>
        /// <param name="client">HttpClient instance</param>
        /// <param name="user">Username to use when authenticating</param>
        /// <param name="pass">Password to use when authenticating</param>
        /// <returns>HttpClient instance with the auth info applied</returns>
        public static HttpClient UseBasicAuth(this HttpClient client, string user, string pass) => client
            .UseBasicAuth(Convert.ToBase64String(Encoding.UTF8.GetBytes("username=" + user + "&password=" + pass + "&grant_type=password")));

        /// <summary>
        /// Tells the HttpClient instance to use the given basic auth info in future requests (encoded)
        /// </summary>
        /// <param name="client">HttpClient instance</param>
        /// <param name="authString">Encoded Auth string to use</param>
        /// <returns>HttpClient instance with the auth info applied</returns>
        public static HttpClient UseBasicAuth(this HttpClient client, string authString) => client.WithAuthHeader("basic", authString);

        /// <summary>
        /// Sets the base Uri (scheme, domain and any port info) to use for all future requests
        /// </summary>
        /// <param name="client">HttpClient instance</param>
        /// <param name="baseUriString">The base Uri to use</param>
        /// <returns>HttpClient instance with the uri info applied</returns>
        public static HttpClient WithBaseUri(this HttpClient client, string baseUriString)
        {
            client.BaseAddress = new Uri(baseUriString);
            return client;
        }

        static HttpClient WithAuthHeader(this HttpClient client, string authType, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authType, token);
            return client;
        }

        /// <summary>
        /// Tells the HttpClient instance to use the given auth token in future requests
        /// </summary>
        /// <param name="client">HttpClient instance</param>
        /// <param name="token">token to use</param>
        /// <returns>HttpClient instance with the auth info applied</returns>
        public static HttpClient WithAuthToken(this HttpClient client, string token) => client.WithAuthHeader("bearer", token);

        /// <summary>
        /// Get a string from the API
        /// </summary>
        /// <param name="client">HttpClient instance</param>
        /// <param name="query">Path to API call</param>
        /// <returns>The requested response as a T</returns>
        public static async ValueTask<T> GetAsync<T>(this HttpClient client, string query)
        {
            var response = await client.GetAsync(query);

            if ((int)response.StatusCode > 399)
                throw new Exception(await response.Content.ReadAsStringAsync());

            return await response.Content.ReadAsAsync<T>();
        }

        /// <summary>
        /// Get a string from the API
        /// </summary>
        /// <param name="client">HttpClient instance</param>
        /// <param name="query">Path to API call</param>
        /// <returns>The requested response stream</returns>
        public static async ValueTask<Stream> GetStreamAsync(this HttpClient client, string query) 
            => await client.GetAsync(query)
                .ContinueWith(t => t.Result.Content.ReadAsStreamAsync())
                .Unwrap();

        /// <summary>
        /// Get a string from the API
        /// </summary>
        /// <param name="client">HttpClient instance</param>
        /// <param name="query">Path to API call</param>
        /// <returns>The requested response string</returns>
        public static async ValueTask<string> GetStringAsync(this HttpClient client, string query) 
            => await client.GetAsync(query)
                .ContinueWith(t => t.Result.Content.ReadAsStringAsync())
                .Unwrap();
    }
}