using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs;

public class TaskDTO
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Title { get; set; } = null!;

    [MaxLength(255)]
    public string? Description { get; set; }

    public bool Completed { get; set; } = false;

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime? Due { get; set; }
}
