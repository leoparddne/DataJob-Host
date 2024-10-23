using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Parameter = RestSharp.Parameter;

namespace DataJob.Server.Helper
{

    public static class HttpHelper
    {
        public static async Task<TResult> GetAsync<TResult>(string url, Dictionary<string, object> parameter = null, Dictionary<string, string> Headers = null, int timeout = 10000, bool statusErrorHandle = true, bool readBodyWhenErr = false)
        {
            return JsonConvert.DeserializeObject<TResult>((await GetAsync(url, parameter, Headers, timeout, statusErrorHandle, readBodyWhenErr)) ?? string.Empty);
        }

        public static async Task<string> GetAsync(string url, Dictionary<string, object> parameter = null, Dictionary<string, string> Headers = null, int timeout = 10000, bool statusErrorHandle = true, bool readBodyWhenErr = false)
        {
            GetInit(url, parameter, Headers, timeout, out var client, out var request);
            return await GetResult(client, request, statusErrorHandle, readBodyWhenErr);
        }

        private static void GetInit(string url, Dictionary<string, object> parameter, Dictionary<string, string> Headers, int timeout, out RestClient client, out RestRequest request)
        {
            client = new RestClient(new RestClientOptions
            {
                Timeout = timeout
            });
            request = new RestRequest(url);
            request.Timeout = timeout;
            GenerateHeader(request, Headers);
            if (parameter == null)
            {
                return;
            }

            foreach (KeyValuePair<string, object> item in parameter)
            {
                Parameter parameter2 = Parameter.CreateParameter(item.Key, item.Value, ParameterType.GetOrPost);
                request.AddParameter(parameter2);
            }
        }

        public static async Task<TResult> PostAsync<TResult>(string url, object obj = null, Dictionary<string, string> Headers = null, int timeout = 10000, bool statusErrorHandle = true, bool readBodyWhenErr = false)
        {
            return JsonConvert.DeserializeObject<TResult>((await PostAsync(url, obj, Headers, timeout, statusErrorHandle, readBodyWhenErr)) ?? string.Empty);
        }

        public static async Task<string> PostAsync(string url, object obj = null, Dictionary<string, string> Headers = null, int timeout = 10000, bool statusErrorHandle = true, bool readBodyWhenErr = false)
        {
            PostInit(url, obj, Headers, timeout, out var client, out var request);
            return await GetResult(client, request, statusErrorHandle, readBodyWhenErr);
        }

        public static async Task<TResult> PostFormDataAsync<TResult>(string url, Dictionary<string, string> Headers = null, Dictionary<string, object> parameters = null, int timeout = 10000, bool statusErrorHandle = true, bool readBodyWhenErr = false)
        {
            return JsonConvert.DeserializeObject<TResult>((await PostFormDataAsync(url, Headers, parameters, timeout, statusErrorHandle, readBodyWhenErr)) ?? string.Empty);
        }

        public static async Task<string> PostFormDataAsync(string url, Dictionary<string, string> Headers = null, Dictionary<string, object> parameters = null, int timeout = 10000, bool statusErrorHandle = true, bool readBodyWhenErr = false)
        {
            PostInit(url, null, Headers, timeout, out var client, out var request);
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value, ParameterType.GetOrPost);
            }

            return await GetResult(client, request, statusErrorHandle, readBodyWhenErr);
        }

        private static async Task<string> GetResult(RestClient client, RestRequest request, bool statusErrorHandle = true, bool readBodyWhenErr = false)
        {
            RestResponse restResponse = await client.ExecuteAsync(request);
            if (!statusErrorHandle || restResponse.IsSuccessful)
            {
                return restResponse.Content;
            }

            if (readBodyWhenErr)
            {
                return restResponse.Content;
            }

            if (restResponse.ErrorException == null)
            {
                if (!string.IsNullOrWhiteSpace(restResponse.ErrorMessage))
                {
                    throw new Exception(restResponse.ErrorMessage);
                }

                throw new Exception("Get response error. " + request.Resource);
            }

            if (restResponse.ErrorException is HttpRequestException ex)
            {
                throw new Exception($"{(int)(ex?.StatusCode).GetValueOrDefault()}:{ex?.Message ?? string.Empty}");
            }

            throw restResponse.ErrorException;
        }

        private static void PostInit(string url, object obj, Dictionary<string, string> Headers, int timeout, out RestClient client, out RestRequest request)
        {
            client = new RestClient(new RestClientOptions
            {
                Timeout = timeout
            });
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = null,
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
            client.UseNewtonsoftJson(settings);
            request = new RestRequest(url, Method.Post);
            request.Timeout = timeout;
            GenerateHeader(request, Headers);
            if (obj != null)
            {
                request.AddBody(obj);
            }
        }

        private static void GenerateHeader(RestRequest request, Dictionary<string, string> Headers)
        {
            if (Headers != null && Headers.Count > 0)
            {
                request.AddOrUpdateHeaders(Headers);
            }
        }
    }
}