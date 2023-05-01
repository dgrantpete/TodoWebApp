using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs;

public class CreateAccountDTO
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required, MinLength(8)]
    public string Password { get; set; } = null!;
}
