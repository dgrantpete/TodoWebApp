namespace TodoApi.Models;

public class TodoTask
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool Completed { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Due { get; set; }
    public int UserAccountId { get; set; }
}
