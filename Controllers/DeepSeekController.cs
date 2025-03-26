using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using DeeoSeekChatApp.Models.DeepSeek;

namespace DeepSeekChatApp.Controllers
{
    public class DeepSeekController : Controller
    {
        private readonly DeepSeekSettings _deepSeekSettings;

        public DeepSeekController(IOptions<DeepSeekSettings> deepSeekSettings)
        {
            _deepSeekSettings = deepSeekSettings.Value;  // Extract the configured settings
        }

        [HttpPost]
        public async Task<ActionResult> Chat(string message)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Use _deepSeekSettings.ApiKey instead of hardcoded value
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_deepSeekSettings.ApiKey}");

                    var requestData = new
                    {
                        model = "deepseek-chat",
                        messages = new[] { new { role = "user", content = message } }
                    };

                    // Use _deepSeekSettings.ApiUrl instead of hardcoded URL
                    var response = await client.PostAsJsonAsync(_deepSeekSettings.ApiUrl, requestData);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadAsAsync<DeepSeekResponse>();
                    return Json(new { success = true, response = result.choices[0].message.content });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        }

    }
