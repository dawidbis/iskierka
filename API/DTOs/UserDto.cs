namespace API.DTOs;

public class UserDto
{
    public required string Id { get; set; }
    public required string EmailAddress { get; set; }
    public required string DisplayName { get; set; }
    public string? ImageUrl { get; set; }
    public required string Token { get; set; }
}
