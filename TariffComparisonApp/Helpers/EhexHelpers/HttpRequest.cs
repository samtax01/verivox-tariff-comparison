using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

// ReSharper disable once CheckNamespace
#pragma warning disable 1570
namespace Ehex.Helpers
{

    /// <summary>
    /// Helper Class
    /// @version: 1.0
    /// @repo: https://github.com/samtax01/ehex-dotnet-helper
    /// </summary>
    public static class HttpRequest
    {
        private static readonly HttpClient Client = new();
        

        /// <summary>
        ///     Create Basic Authentication to a request header.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="httpClient">optional as HttpService.Client</param>
        /// <returns></returns>
        public static HttpClient AddBasicAuth(string userName, string password, HttpClient httpClient = null)
        {
            httpClient ??= Client;
            var authToken = Encoding.ASCII.GetBytes($"{userName}:{password}");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
            return httpClient;
        }


        /// <summary>
        /// Make Request with IHttpClientFactory
        /// Make a request and get a string response.
        /// This string can later be converted to JSON of an object...
        /// 
        /// Register and IHttpClientFactory clientFactory
        ///     And [ To bypass SSL certificate [Error: The SSL connection could not be established] ]
        ///     services.AddHttpClient(Options.DefaultName, c => { }).ConfigurePrimaryHttpMessageHandler(() =>
        ///     {
        ///         return new HttpClientHandler
        ///         {
        ///             ClientCertificateOptions = ClientCertificateOption.Manual,
        ///             ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, policyErrors) => true
        ///         };
        ///     });
        /// 
        /// <br/>
        /// 
        ///  Sample Usage:
        ///     var responseMessage = await HttpRequest.Make("https://localhost:5001/api/v1/pingin.com/2");
        ///     var apiResponse = await ApiResponse{MyModel}.FromRequest(responseMessage);
        ///     Console.Write(apiResponse);
        /// 
        /// <pre>
        ///     - Convert to ApiResponse ApiResponse{dynamic}.FromRequest(result)
        ///     - Get string value with "response.Content.ReadAsStringAsync()"
        ///     - Use JsonConvert.DeserializeObject{dynamic}( response.Content ) to convert to a dynamic object.
        ///     - Use ApiResponse{dynamic}.FromRequest(response) to normalize value.
        /// </pre>
        /// 
        /// </summary>
        /// <param name="clientFactory">DI singleton instance of IHttpClientFactory clientFactory</param>
        /// <param name="url">site address</param>
        /// <param name="httpRequestMethod">Default is HttpMethod.Get</param>
        /// <param name="requestData">Request paload</param>
        /// <param name="headers">List of Header content</param>
        /// <returns>Returned HttpResponseMessage</returns>
        public static async Task<HttpResponseMessage> MakeAsync(
            IHttpClientFactory clientFactory,
            string url,
            HttpMethod httpRequestMethod = null,
            object requestData = null,
            IDictionary<string, string> headers = null
        )
        {
            try
            {
                // Add Headers
                var request = new HttpRequestMessage(httpRequestMethod ?? HttpMethod.Get, url);
                if(headers != null)
                    foreach (var (key, value) in headers)
                        request.Headers.Add(key, value);
                
                // Add Request Data
                if (httpRequestMethod != HttpMethod.Get && requestData is not null)
                    request.Content = new StringContent(requestData is string data ? data: JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

                // Make httpClient and request
                var client = clientFactory.CreateClient();
                return await client.SendAsync(request);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occurred while making request to {0} - Detail: {1}", url, e);
                throw;
            }
        }


        /// <summary>
        /// Make Request with HTTPClient
        /// 
        /// To bypass SSL certificate [Error: The SSL connection could not be established]
        ///    var clientHandler = new HttpClientHandler();
        ///    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        ///    _httpClient = new HttpClient(clientHandler);
        /// </summary>
        /// <param name="httpClient">Instance of HttpClient httpClient</param>
        /// <param name="url">site address</param>
        /// <param name="httpRequestMethod">Default is HttpMethod.Get</param>
        /// <param name="requestData">Request payload</param>
        public static async Task<HttpResponseMessage> MakeAsync(
            HttpClient httpClient, 
            string url, 
            HttpMethod httpRequestMethod = null, 
            object requestData = null)
        {
            try
            {
                HttpResponseMessage response;
                if (httpRequestMethod == HttpMethod.Post)
                    response = await httpClient.PostAsJsonAsync(url, requestData!);
                else if (httpRequestMethod == HttpMethod.Put)
                    response = await httpClient.PutAsJsonAsync(url, requestData!);
                else
                    response = httpClient.GetAsync(url).Result;
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occurred while making request to {0} - Detail: {1}", url, e);
                throw;
            }
        }


        public static bool IsUrlValid(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&  (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

    }
}