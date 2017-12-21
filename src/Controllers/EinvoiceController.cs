using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using EinvoiceApiTest.Utility;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EinvoiceApiTest.Model;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace EinvoiceApiTest.Controllers
{
    [Produces("application/json")]
    [Route("api/Einvoice")]
    public class EinvoiceController : Controller
    {
        public readonly IConfiguration _configuration = null;

        public EinvoiceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ApiResult Get()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Json", "ApiList.json");
            string json = System.IO.File.ReadAllText(filePath);

            var models = JsonConvert.DeserializeObject<List<ApiListModel>>(json);

            string appKey = _configuration.GetValue<string>("AppKey");
            string appID = _configuration.GetValue<string>("AppID");

            string timestamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds.ToString("0");
            string uuid = Guid.NewGuid().ToString().Replace("-", "");

            for (int i = 0; i < models.Count; i++)
            {
                models[i].Id = (i + 1);

                if (models[i].Param != null)
                {
                    var formData = models[i].Param;
                    if (formData.Keys.Contains("timeStamp"))
                    {
                        formData["timeStamp"] = timestamp;
                    }

                    if (formData.Keys.Contains("uuid"))
                    {
                        formData["uuid"] = uuid;
                    }

                    if (formData.Keys.Contains("appID"))
                    {
                        formData["appID"] = appID;
                    }
                }
            }

            return apiResult(1, new
            {
                appKey,
                apiList = models.OrderBy(x => x.TypeName)
            });
        }

        [HttpPost]
        public ApiResult Post(PostModel post)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.Where(x => x.Value.Errors.Count > 0)
                                          .ToDictionary(x => x.Key,
                                                        x => x.Value.Errors.Select(e => e.ErrorMessage));
                return apiResult(0, errorList);
            }

            try
            {
                var formData = JsonConvert.DeserializeObject<Dictionary<string, string>>(post.ApiRequest);

                // 需加入簽章
                if (post.IsNeedSignature)
                {
                    var tmp = formData.OrderBy(x => x.Key)
                                      .Select(x => string.Format("{0}={1}", x.Key, x.Value));
                    string signature = CryptUtility.HMACSHA1(post.ApiKey, string.Join("&", tmp));
                    formData.Add("signature", signature);
                }

                // 財政部測試環境
                if (post.IsTestMode)
                {
                    post.ApiUrl = post.ApiUrl.Replace("//www.", "//wwwtest.")
                                             .Replace("//api.", "//wwwtest.");
                }

                var query = formData.Select(x => string.Format("{0}={1}", x.Key, HttpUtility.UrlEncode(x.Value)));
                string url = string.Format("{0}?{1}", post.ApiUrl.TrimEnd('?'), string.Join("&", query));

                if (post.IsClientMode)
                {
                    return apiResult(100, url);
                }
                else
                {
                    string result = null;
                    using (HttpClient hc = new HttpClient())
                    {
                        // 30 秒 Time Out
                        hc.Timeout = TimeSpan.FromSeconds(30);
                        result = hc.GetStringAsync(url).Result;
                    }
                    return apiResult(1, new
                    {
                        url,
                        result
                    });
                }
            }
            catch (Exception ex)
            {
                return apiResult(-1, ex.Message);
            }
        }

        private ApiResult apiResult(int rtnCode, object rtnData)
        {
            return new ApiResult
            {
                RtnCode = rtnCode,
                RtnData = rtnData
            };
        }
    }
}