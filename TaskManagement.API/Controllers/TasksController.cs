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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var task = await _service.GetByIdAsync(id);
        
        if (task == null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(string title, string description)
    {
        var task = await _service.CreateTaskAsync(title, description);
        return Ok(task);
    }
}