using Microsoft.Identity.Client;
using System.ComponentModel;

namespace Mvc_Web_Connected.Models
{
    public class EmpDept
    {
        [DisplayName("Employee Name")]
       public string EmpName {  get; set; }
        [DisplayName("Department Name")]
        public string DeptName {  get; set; }
        [DisplayName("Library Name")]
        public string LibName {  get; set; }
    }
}
