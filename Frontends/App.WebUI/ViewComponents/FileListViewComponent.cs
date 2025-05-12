using App.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.WebUI.ViewComponents
{
    public class FileListViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FileListViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var response = await client.GetAsync($"/api/files/filters/public");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var fileList = await response.Content.ReadFromJsonAsync<List<FileListViewModel>>();

            if (fileList == null)
            {
                return View("Error");
            }

            var orderList = fileList
                .OrderByDescending(x => x.Id)
                .Take(6)
                .ToList();
            return View(orderList);
        }
    }
}
