using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Mvc_Web_Connected.Models;
using Newtonsoft.Json;

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
            var emp = await GetEmployeesAsync();

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
        private static async Task<List<Employee>> GetEmployeesAsync()
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
                return System.Text.Json.JsonSerializer.Deserialize<List<Employee>>(responseData,
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
                var isSuccess = await PostEmployeesAsync(emp);

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

        private async Task<bool> PostEmployeesAsync(Employee emp)
        {
            HttpClient client = new HttpClient();
            // Set base address and headers
            client.BaseAddress = new Uri("https://localhost:7183/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Serialize the department object to JSON
            var json = System.Text.Json.JsonSerializer.Serialize(emp);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the POST request to the Web API
            HttpResponseMessage response = await client.PostAsync("api/Employee1", content);

            // Return true if the request was successful
            return response.IsSuccessStatusCode;
        }
        //Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        { 
            //Employee employee = new Employee();
             var employee = await GetEmployeesAsync(id);
            return View("Edit",employee);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            id = employee.EmpId;
            HttpClient client = new HttpClient();

            string apiUrl = $"https://localhost:7183/api/Employee1/{id}";
            var jsonContent = JsonConvert.SerializeObject(employee);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // or appropriate action
            }
            else
            {
                ModelState.AddModelError("", "Error updating employee.");
                return View("Edit",employee);
            }
        }
        private static async Task<Employee> GetEmployeesAsync(int id)
        {
            HttpClient client = new HttpClient();
            // Set the base address of the API
            client.BaseAddress = new Uri("https://localhost:7183/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync($"api/Employee1/{id}");
            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the JSON response to a List<Employee>
                var responseData = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<Employee>(responseData,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                throw new Exception("Failed to retrieve employee from API");
            }
        }
        //Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await GetEmployeeByIdAsync(id); // Retrieve the department by ID

            return View("Delete", employee); // Pass the department to the Delete view for confirmation
        }

        // POST: Home/DeleteConfirmed/{id}
        [HttpPost, ActionName("DeletedConfirmed")]
        [Route("Home/DeletedConfirmed/{id}")]
        public async Task<IActionResult> DeletedConfirmed(int id)
        {
            var isSuccess = await DeleteEmployeeAsync(id); // Call method to delete the department
            if (isSuccess)
            {
                return RedirectToAction("Index"); // Redirect to Index if deletion is successful
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to delete employee.");
            }
            return RedirectToAction("Index"); // Return to the delete view if there was an error
        }
        private async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:7183/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync($"api/Employee1/{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<Employee>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            else
            {
                throw new Exception("Failed to delete from api.");
            }
        }

        // Method to call the Web API and delete a department
        private async Task<bool> DeleteEmployeeAsync(int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7183/"); // Ensure this matches your API's base URL
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.DeleteAsync($"api/Employee1/{id}"); // Send the DELETE request
            return response.IsSuccessStatusCode; // Return true if successful
        }
    }
}

