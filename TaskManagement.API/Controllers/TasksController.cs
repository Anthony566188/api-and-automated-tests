using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Entities;
using TaskManagement.API.Interfaces;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("[controller]")] 
public class TasksController : ControllerBase 
{
    private readonly ITaskService _service;

    public TasksController(ITaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _service.GetAllAsync();
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(string title, string description)
    {
        var task = await _service.CreateTaskAsync(title, description);
        return Ok(task);
    }
}