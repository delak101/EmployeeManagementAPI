using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementAPI.Entities;

public class Employee : User
{        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        public DateTime BirthDate { get; set; }
        
        [Required]
        public string Position { get; set; }
        
        // Foreign key for the relationship
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        public bool IsDeleted { get; set; }

        // Navigation property
        public Department Department { get; set; }

}
