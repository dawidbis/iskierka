using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class LoginDto
{
    public string EmailAddress { get; set; } = "";
    public string Password { get; set; } = "";
}
