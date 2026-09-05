using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;
using TaskManagement.API.Entities;
using TaskManagement.API.Interfaces;
using TaskManagement.API.Diagnostics;

namespace TaskManagement.API.Services;

public class TaskService : ITaskService
{
    // Lista em memória
    private static readonly List<TaskItem> _tasks = new();
    private readonly ILogger<TaskService> _logger;

    // TRACING: ActivitySource é a classe nativa do .NET para criar "Spans"
    private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName); 

    // MÉTRICAS: Counters nativos do .NET
    private readonly Counter<int> _tasksCreatedCounter;

    public TaskService(ILogger<TaskService> logger, IMeterFactory meterFactory) 
    {
        _logger = logger;

        var meter = meterFactory.Create(TelemetryConstants.MeterName); 
        _tasksCreatedCounter = meter.CreateCounter<int>("tasks_created_total", description: "Total de tarefas criadas"); 
    }

    public async Task<TaskItem> CreateTaskAsync(string title, string description)
    {

        // 1. TRACING: Iniciando uma nova atividade (Span) associada a esta requisição
        using var activity = ActivitySource.StartActivity("CreateTask");
        activity?.SetTag("task.title", title);

        // Simulando delay de banco de dados
        await Task.Delay(100); 

        var task = new TaskItem { Title = title, Description = description };
        _tasks.Add(task);

        // 2. LOG ESTRUTURADO
        _logger.LogInformation("Nova tarefa criada com sucesso: {@TaskItem}", task);

        // 3. MÉTRICA: Incrementando o contador com tags (labels)
        _tasksCreatedCounter.Add(1, new KeyValuePair<string, object?>("title", title)); 
        return task;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {

        using var activity = ActivitySource.StartActivity("GetTaskById");
        activity?.SetTag("task.id", id.ToString());

        await Task.Delay(100);
        return _tasks.FirstOrDefault(t => t.Id == id);
    }
}