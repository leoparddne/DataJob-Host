using DataJob.Server.Helper;
using DataJob.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataJob.Server.Util
{
    public static class HttpUtil
    {
        public static T Get<T>(string url)
        {
            Task<T> result = HttpHelper.GetAsync<T>(url);

            result.Wait();

            return result.Result;
        }
        public static T APIGet<T>(string url)
        {
            var result = Get<APIResponseModel<T>>(url);

            return GetData(result);
        }

        public static T Post<T>(string url, object obj = null, int timeOut = 10)
        {
            Task<T> result = HttpHelper.PostAsync<T>(url, obj, null, timeOut * 1000);
            result.Wait();

            return result.Result;
        }

        public static T APIPost<T>(string url, object obj = null, int timeOut = 10)
        {
            var result = Post<APIResponseModel<T>>(url, obj, timeOut);

            return GetData(result);
        }


        public static T GetData<T>(APIResponseModel<T> data)
        {
            ExceptionHelper.CheckException(data.Code != (int)StatesEnum.Y, data.Message);

            return data.Data;
        }


        public static string BindUrl(string baseUrl, string url)
        {
            baseUrl = baseUrl.TrimEnd('/');
            url = url.TrimStart('/');

            return baseUrl + '/' + url;

        }
    }
}