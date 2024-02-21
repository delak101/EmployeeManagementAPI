using EmployeeManagementAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { 

    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<User> Users { get; set; }

    // public object Users { get; internal set; }
    //     protected override void OnModelCreating(ModelBuilder modelBuilder)
    //     {
    //         // Configure your entity relationships and constraints here
    //     }
}

