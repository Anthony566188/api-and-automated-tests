namespace TaskManagement.API.Entities;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid(); // Identificador único
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}