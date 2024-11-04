using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mvc_Web_Connected.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace Mvc_Web_Connected.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private static readonly HttpClient client = new HttpClient();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await GetDepartmentsAsync();

            return View("DepartmentView", departments);
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
        public IActionResult Create()
        {
            Department department = new Department();
            return View("Create", department);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            
                if (ModelState.IsValid)
                {
                    var isSuccess = await PostDepartmentAsync(department);

                    if (isSuccess)
                    {
                        return RedirectToAction("Index");  // Redirect to Index or success page
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to insert department.");
                    }
                }

            return View(department);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Department department = new Department();
            department = await GetDepartmentsAsync(id) ;
            return View("Edit", department);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDepartment(int id, Department department)
        {
            id = department.DeptId;
            HttpClient client = new HttpClient();
         
            string apiUrl = $"https://localhost:7183/api/Department1/{id}";
            var jsonContent = JsonConvert.SerializeObject(department);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // or appropriate action
            }
            else
            {
                ModelState.AddModelError("", "Error updating department.");
                return View(department);
            }
        }

        private static async Task<Department> GetDepartmentsAsync(int id)
        {
            HttpClient client = new HttpClient();
            // Set the base address of the API
            client.BaseAddress = new Uri("https://localhost:7183/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync($"api/Department1/{id}");
            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the JSON response to a List<Department>
                var responseData = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<Department>(responseData,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                throw new Exception("Failed to retrieve departments from API");
            }
        }
        private static async Task<List<Department>> GetDepartmentsAsync(  )
        {
            HttpClient client = new HttpClient();
            // Set the base address of the API
            client.BaseAddress = new Uri("https://localhost:7183/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response =  await client.GetAsync("api/Department1");
            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the JSON response to a List<Department>
                var responseData = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<List<Department>>(responseData,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                throw new Exception("Failed to retrieve departments from API");
            }
        }

        private async Task<bool> PostDepartmentAsync(Department department)
        {
            HttpClient client = new HttpClient();
            // Set base address and headers
            client.BaseAddress = new Uri("https://localhost:7183/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Serialize the department object to JSON
            var json = System.Text.Json.JsonSerializer.Serialize(department);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the POST request to the Web API
            HttpResponseMessage response = await client.PostAsync("api/Department1", content);

            // Return true if the request was successful
            return response.IsSuccessStatusCode;
        }

        
    }
}
