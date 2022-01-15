using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimonVossTask.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimonVossTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;

            this.httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            //var model_data = TempData["Message"] as ResultViewModel;
            //if (TempData["Message"] is string s)
            //{
            //    var model_data = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultViewModel>(s);
            //    return View(model_data);
            //}
            //var model = TempData["Message"] as List<ResultViewModel>;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Search(IFormCollection values)
        {
            try
            {
                var client = httpClientFactory.CreateClient("CustomersService");
                var response = await client.GetAsync($"api/search/{values["search_term"]}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<dynamic>(content);

                    TempData["Message"] = result.ToString();
                    return RedirectToAction("Index", "Home");

                }
                NotFound();
            }
            catch (Exception ex)
            {

                NotFound(ex.ToString());
            }

            return RedirectToAction("Privacy", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
