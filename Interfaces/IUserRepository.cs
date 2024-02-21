using EmployeeManagementAPI.Entities;

namespace EmployeeManagementAPI.Interfaces;

public interface IUserRepository
{
        Task AddUserAsync(User user);
        Task<bool> IsUsernameAvailableAsync(string username);
        Task<bool> IsValidUserAsync(string username, string password);
        Task<bool> IsValidUserByIdAsync(int userId, string password);
        Task<User> GetUserByIdAsync(int userId);
        Task<int> GetUserIdByUsernameAsync(string username);
        Task<User> GetUserByUsernameAsync(string username);
        Task UpdateUserAsync(User user);
        Task UpdateUserPasswordAsync(int userId, string newPassword);
        string HashPassword(string password);
        bool IsUsernameValid(string username);
        bool IsPasswordValid(string password);

}
