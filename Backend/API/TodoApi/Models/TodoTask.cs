using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models;

public class TodoTask
{
    [Required, Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Title { get; set; } = null!;

    [MaxLength(255)]
    public string? Description { get; set; }

    [Required]
    public bool Completed { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Due { get; set; }

    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public TodoUser User { get; set; } = null!;
}
