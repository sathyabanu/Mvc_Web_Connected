using System.ComponentModel;

namespace Mvc_Web_Connected.Models
{
    public class Employee
    {
        [DisplayName("Employee Id")]
        public int EmpId { get; set; }
        [DisplayName("Employee Name")]
        public string EmpName { get; set; }
        [DisplayName("Department Id")]
        public int DeptId { get; set; }
        [DisplayName("Library Id")]
        public int LibId { get; set; }
    }
}
