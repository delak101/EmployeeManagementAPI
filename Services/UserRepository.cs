using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Entities;
using EmployeeManagementAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Services;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddUserAsync(User user)
    {
        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsUsernameAvailableAsync(string username)
    {
        return !await _dbContext.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<bool> IsValidUserAsync(string username, string password)
    {
        var hashedPassword = HashPassword(password);
        return await _dbContext.Users.AnyAsync(u => u.Username == username && u.HashedPassword == hashedPassword);
    }

    public async Task<bool> IsValidUserByIdAsync(int userId, string password)
    {
        var hashedPassword = HashPassword(password);
        return await _dbContext.Users.AnyAsync(u => u.Id == userId && u.HashedPassword == hashedPassword);
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
    
    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<int> GetUserIdByUsernameAsync(string username)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        return user?.Id ?? 0; // Assuming 0 as a default value if not found
    }

    public async Task UpdateUserAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateUserPasswordAsync(int userId, string newPassword)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            user.HashedPassword = newPassword;
            await _dbContext.SaveChangesAsync();
        }
    }

    public string HashPassword(string password)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            byte[] salt = hmac.Key;
            byte[] hashedPasswordBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            byte[] combinedBytes = salt.Concat(hashedPasswordBytes).ToArray();
            return Convert.ToBase64String(combinedBytes);
        }
    }

    public bool IsUsernameValid(string username)
    {
        // Basic validation: Check if the username is between 3 and 20 characters
        return !string.IsNullOrEmpty(username) && username.Length >= 3 && username.Length <= 20;
    }

    public bool IsPasswordValid(string password)
    {
        // Basic validation: Check if the password is at least 6 characters
        return !string.IsNullOrEmpty(password) && password.Length >= 6;
    }
}
