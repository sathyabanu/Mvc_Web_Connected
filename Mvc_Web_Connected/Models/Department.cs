using System.ComponentModel;

namespace Mvc_Web_Connected.Models
{
    public partial class Department
    {
        [DisplayName("Department Id")]
        public int  DeptId { get; set; }
        [DisplayName("Department Name")]
        public string DeptName { get; set; } = null!;
        [DisplayName("Department Location")]
        public string DeptLoc { get; set; } = null!;

    }
}
