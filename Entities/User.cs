using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.Entities;

public class User
{
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string HashedPassword { get; set; }
}
