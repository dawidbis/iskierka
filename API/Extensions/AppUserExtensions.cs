using API.DTOs;
using API.Entities;
using API.Services.Interfaces;

namespace API.Extensions;

public static class AppUserExtensions
{
    public static UserDto ToDto(this AppUser user, ITokenService tokenService)
    {
        return new UserDto
        {
            Id = user.Id,
            EmailAddress = user.EmailAddress,
            DisplayName = user.DisplayName,
            Token = tokenService.CreateToken(user)
        };
    }
}
