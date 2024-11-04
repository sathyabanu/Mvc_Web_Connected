using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Mvc_Web_Connected.Models;


namespace Api_Web_emp.Controllers
{
    public class EmpController : Controller
    {
        private readonly ILogger<EmpController> _logger;

        public EmpController(ILogger<EmpController> logger)
        {
            _logger =logger;
        }

        public async Task<IActionResult> Index()
        {
            var emp = await GetEmployeeAsync();

            return View("EmployeeView", emp);
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
        private static async Task<List<Employee>> GetEmployeeAsync()
        {
            HttpClient client = new HttpClient();
            // Set the base address of the API
            client.BaseAddress = new Uri("https://localhost:7183/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Call the API
            HttpResponseMessage response = await client.GetAsync("api/Employee1");

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the JSON response to a List<Department>
                var responseData = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Employee>>(responseData,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                throw new Exception("Failed to retrieve employee from API");
            }
        }
        public IActionResult Create()
        {
            Employee emp = new Employee();
            return View("Create", emp);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
                var isSuccess = await PostEmployeeAsync(emp);

                if (isSuccess)
                {
                    return RedirectToAction("Index");  // Redirect to Index or success page
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to insert Employee.");
                }
            }
            return View(emp);
        }

        private async Task<bool> PostEmployeeAsync(Employee emp)
        {
            HttpClient client = new HttpClient();
            // Set base address and headers
            client.BaseAddress = new Uri("https://localhost:7183/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Serialize the department object to JSON
            var json = JsonSerializer.Serialize(emp);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the POST request to the Web API
            HttpResponseMessage response = await client.PostAsync("api/Employee1", content);

            // Return true if the request was successful
            return response.IsSuccessStatusCode;
        }




    }
}

