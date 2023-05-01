using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;

[Index(nameof(Email), IsUnique = true)]
public class TodoUser
{
    [Required, Key]
    public int Id { get; set; }

    [Required, MaxLength(255), EmailAddress]
    public string Email { get; set; } = null!;

    [Required, MaxLength(255)]
    public string PasswordHash { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public ICollection<TodoTask> Tasks { get; set; } = null!;
}