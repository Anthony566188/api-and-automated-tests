using Microsoft.Extensions.Logging;
using TaskManagement.API.Entities;
using TaskManagement.API.Interfaces;

namespace TaskManagement.API.Services;

public class TaskService : ITaskService
{
    // Lista em memória
    private static readonly List<TaskItem> _tasks = new();
    private readonly ILogger<TaskService> _logger;

    public TaskService(ILogger<TaskService> logger)
    {
        _logger = logger;
    }

    public async Task<TaskItem> CreateTaskAsync(string title, string description)
    {
        // Simulando delay de banco de dados
        await Task.Delay(100); 

        var task = new TaskItem { Title = title, Description = description };
        _tasks.Add(task);

        // O uso de {@TaskItem} diz ao logger para serializar o objeto como JSON
        _logger.LogInformation("Nova tarefa criada com sucesso: {@TaskItem}", task);

        return task;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        await Task.Delay(100);
        return _tasks.FirstOrDefault(t => t.Id == id);
    }
}