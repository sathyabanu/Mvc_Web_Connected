using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using Mvc_Web_Connected.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Mvc_Web_Connected.Controllers
{
    public class EmpDeptController : Controller
    {
        private readonly ILogger<EmpDeptController> _logger;

        public EmpDeptController(ILogger<EmpDeptController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var empdepts = await GetEmpDeptsAsync();

            return View("EmpDeptView", empdepts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static async Task<List<EmpDept>> GetEmpDeptsAsync()
        {
            HttpClient client = new HttpClient();
            // Set the base address of the API
            client.BaseAddress = new Uri("https://localhost:7183");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Call the API
            HttpResponseMessage response = await client.GetAsync("api/Join");

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the JSON response to a List<Department>
                var responseData = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<EmpDept>>(responseData,
                 new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                throw new Exception("Failed to retrieve empdept from API");
            }
        }
    }
}

