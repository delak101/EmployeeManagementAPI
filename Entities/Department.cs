using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.Entities;

public class Department
{
        [Key]
        public int DepartmentId { get; set; }
        
        [Required]
        public string DepartmentName { get; set; }
        
        public int ManagerId { get; set; }

        // Navigation property for employees
        public ICollection<Employee> Employees { get; set; }
}
