using TaskManagement.API.Entities;

namespace TaskManagement.API.Interfaces;

public interface ITaskService
{
    Task<TaskItem> CreateTaskAsync(string title, string description);
    Task<IEnumerable<TaskItem>> GetAllAsync();
}