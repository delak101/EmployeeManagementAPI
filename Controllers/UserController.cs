using EmployeeManagementAPI.DTOs;
using EmployeeManagementAPI.Entities;
using EmployeeManagementAPI.Interfaces;
using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public UsersController(ITokenService tokenService, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        try
        {
            // Robust validation for username
            if (!_userRepository.IsUsernameValid(registerDto.Username))
            {
                return BadRequest("Invalid username. Please choose a different username.");
            }

            // Robust validation for password
            if (!_userRepository.IsPasswordValid(registerDto.Password))
            {
                return BadRequest("Invalid password. Password must meet the required criteria.");
            }

            // Check if the username is available
            if (!await _userRepository.IsUsernameAvailableAsync(registerDto.Username))
            {
                return BadRequest("Username already exists");
            }

            // Hash and salt the password before storing it
            var hashedPassword = _userRepository.HashPassword(registerDto.Password);
    
            // Generate a unique user ID (let the database handle it)
            var newUser = new User 
            { 
                Username = registerDto.Username, 
                HashedPassword = hashedPassword 
            };
            await _userRepository.AddUserAsync(newUser);

            // Access the generated ID after saving to the database
            var userId = newUser.Id;


            // Generate a token for the newly registered user
            var user = await _userRepository.GetUserByIdAsync(userId);
            var token = _tokenService.GenerateToken(user);

            var userDto = new UserDto
            {
                Id = userId,
                Username = user.Username,
            };

            return Ok(new { Token = token, User = userDto });
        }
        catch (Exception)   
        {
            // Handle database exception or other errors
            return StatusCode(500, "Error occurred while registering the user.");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        try
        {
            // Robust validation for username
            if (!_userRepository.IsUsernameValid(loginDto.Username))
            {
                return BadRequest("Invalid username");
            }

            // Robust validation for password
            if (!_userRepository.IsPasswordValid(loginDto.Password))
            {
                return BadRequest("Invalid password");
            }

            // Validate against user data in the database
            if (await _userRepository.IsValidUserAsync(loginDto.Username, loginDto.Password))
            {
                var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);

                // Generate a token for the authenticated user
                var token = _tokenService.GenerateToken(user);

                return Ok(new { Token = token});
            }

            return Unauthorized("Invalid username or password");
        }
        catch (Exception)
        {
            // Handle database exception or other errors
            return StatusCode(500, "Error occurred during login.");
        }
    }

}
