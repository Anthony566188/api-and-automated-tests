using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;
using TaskManagement.API.Entities;
using TaskManagement.API.Interfaces;
using TaskManagement.API.Diagnostics;

namespace TaskManagement.API.Services;

public class TaskService : ITaskService
{
    private static readonly List<TaskItem> _tasks = new();
    private readonly ILogger<TaskService> _logger;

    // TRACING: ActivitySource criar "Spans"
    private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName); 

    // MÉTRICAS: Counters
    private readonly Counter<int> _tasksCreatedCounter;

    public TaskService(ILogger<TaskService> logger, IMeterFactory meterFactory) 
    {
        _logger = logger;

        var meter = meterFactory.Create(TelemetryConstants.MeterName); 
        _tasksCreatedCounter = meter.CreateCounter<int>("tasks_created_total", description: "Total de tarefas criadas");  
    }

    public async Task<TaskItem> CreateTaskAsync(string title, string description)
    {
        using var activity = ActivitySource.StartActivity("CreateTask");  
        activity?.SetTag("task.title", title);  

        try
        {
            // Simulando delay de banco de dados
            await Task.Delay(100); 

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("O título não pode ser nulo ou vazio.");
            }

            var task = new TaskItem { Title = title, Description = description };
            _tasks.Add(task);

            // LOG DE SUCESSO
            _logger.LogInformation("Nova tarefa criada com sucesso: {@TaskItem}", task);  

            _tasksCreatedCounter.Add(1, new KeyValuePair<string, object?>("title", title));  
            return task;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao tentar criar a tarefa com título: {Title}", title);  
            
            // Re-lança a exceção para que o controlador/middleware saiba que falhou
            throw; 
        }
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        using var activity = ActivitySource.StartActivity("GetAllTasks");  

        try
        {
            await Task.Delay(100);
            return _tasks;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro inesperado ao tentar buscar a lista de tarefas.");  
            throw;
        }
    }
}