using Client.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Reflection;

namespace Client.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _announcementsUrl= "api/announcements";
        public AnnouncementsController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("AnnouncementAPI");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_announcementsUrl);

            if (response.IsSuccessStatusCode)
            {
                var announcements = await response.Content.ReadFromJsonAsync<List<AnnouncementDTO>>();
                return View(announcements);
            }

            return View(new List<AnnouncementDTO>());
        }
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_announcementsUrl}/{id.ToString()}");

            if (response.IsSuccessStatusCode)
            {
                var announcement = await response.Content.ReadFromJsonAsync<AnnouncementDTO>();
                return View(announcement);
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AnnouncementDTO announcementDTO)
        {
            if (!ModelState.IsValid)
                return View(announcementDTO);

            var token = await HttpContext.GetTokenAsync("id_token");
            if(!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var request = await _httpClient.PostAsJsonAsync(_announcementsUrl, announcementDTO);

            if (request.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Помилка при створенні оголошення");
            return View(announcementDTO);
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_announcementsUrl}/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var model = await response.Content.ReadFromJsonAsync<AnnouncementDTO>();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(AnnouncementDTO announcementDTO)
        {
            var token = await HttpContext.GetTokenAsync("id_token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var request = await _httpClient.PutAsJsonAsync($"{_announcementsUrl}/{announcementDTO.Id}", announcementDTO);

            if (request.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Get),new {id = announcementDTO.Id});
            }

            return View(announcementDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var token = await HttpContext.GetTokenAsync("id_token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var request = await _httpClient.DeleteAsync($"{_announcementsUrl}/{id}");

            if (request.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

