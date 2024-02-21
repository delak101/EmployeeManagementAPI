using EmployeeManagementAPI.Entities;

namespace EmployeeManagementAPI.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
