using API.Data;
using API.Entities;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Services.Interfaces;
using API.Extensions;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService) : BaseApiController() // localhost:5001/api/account
{

#region Register Helper Methods
    private async Task<bool> EmailIsTaken(string emailAddress)
    {
        return await context.Users.AnyAsync(x => x.EmailAddress.ToLower() == emailAddress.ToLower());
    }

    private static async Task<AppUser> CreateUser(RegisterDto registerDto)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();

        var user = new AppUser
        {
            EmailAddress = registerDto.EmailAddress,
            DisplayName = registerDto.DisplayName,
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        return user;
    }

    private async Task AddUserToDatabase(AppUser user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }
#endregion

    [HttpPost("register")] // localhost:5001/api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        // Check if the email is already taken
        if (await EmailIsTaken(registerDto.EmailAddress)) return BadRequest("Email taken.");

        // Create a new user
        var user = await CreateUser(registerDto);

        // Add the user to the database
        await AddUserToDatabase(user);

        // Return the user DTO with the token
        return user.ToDto(tokenService);
    }

#region Login Helper Methods
    private async Task<AppUser?> GetUserByEmail(string emailAddress)
    {
        return await context.Users.SingleOrDefaultAsync(x => x.EmailAddress.ToLower() == emailAddress.ToLower());
    }

    private static async Task<bool> VerifyPassword(AppUser user, string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return false;
        }

        return true;
    }
#endregion

    [HttpPost("login")] // localhost:5001/api/account/login
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        // Get the user by email address
        var user = await GetUserByEmail(loginDto.EmailAddress);

        // If the user is not found, return Unauthorized
        if (user == null) return Unauthorized("Invalid email address.");

        // Verify the password
        if (!await VerifyPassword(user, loginDto.Password)) return Unauthorized("Invalid password.");

        // Return the user DTO with the token
        return user.ToDto(tokenService);
    }
}
