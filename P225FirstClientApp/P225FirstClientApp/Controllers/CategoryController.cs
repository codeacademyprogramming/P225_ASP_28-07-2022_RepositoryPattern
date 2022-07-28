using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using P225FirstClientApp.ViewModels.CategoryVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace P225FirstClientApp.Controllers
{
    public class CategoryController : Controller
    {
        public async Task<IActionResult> Index()
        {
            string categoryListUrl = "http://localhost:34689/api/categories";

            HttpResponseMessage httpResponseMessage = null;

            using(HttpClient httpClient = new HttpClient())
            {
                string token = Request.Cookies["AuthToken"];

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                httpResponseMessage = await httpClient.GetAsync(categoryListUrl);
            }

            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                string content = await httpResponseMessage.Content.ReadAsStringAsync();

                List<CategoryIndexVM> categoryIndexVMs = JsonConvert.DeserializeObject<List<CategoryIndexVM>>(content);

                return View(categoryIndexVMs);
            }

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM categoryCreateVM)
        {
            categoryCreateVM.Sekil = "Test.jpg";

            string categoryPostUrl = "http://localhost:34689/api/categories";

            HttpResponseMessage httpResponseMessage = null;

            using (HttpClient httpClient = new HttpClient())
            {
                string token = Request.Cookies["AuthToken"];

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string content = JsonConvert.SerializeObject(categoryCreateVM);

                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                multipartFormDataContent.Add(new StreamContent(categoryCreateVM.File.OpenReadStream()), "File", categoryCreateVM.File.FileName);
                multipartFormDataContent.Add(new StringContent(categoryCreateVM.Ad,Encoding.UTF8,"application/json"), "Ad");
                multipartFormDataContent.Add(new StringContent(categoryCreateVM.Esasdirmi.ToString(), Encoding.UTF8, "application/json"), "Esasdirmi");
                multipartFormDataContent.Add(new StringContent(categoryCreateVM.Sekil, Encoding.UTF8, "application/json"), "Sekil");

                if (categoryCreateVM.AidOlduguKategoriyaninIdsi != null)
                {
                    multipartFormDataContent.Add(new StringContent(categoryCreateVM.AidOlduguKategoriyaninIdsi?.ToString(), Encoding.UTF8, "application/json"), "AidOlduguKategoriyaninIdsi");
                }

                httpResponseMessage = await httpClient.PostAsync(categoryPostUrl, multipartFormDataContent);
            }

            if (httpResponseMessage.StatusCode == HttpStatusCode.Created)
            {
                return RedirectToAction("index");
            }

            return RedirectToAction("index");
        }
    }
}
