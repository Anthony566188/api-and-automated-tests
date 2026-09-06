using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Entities;
using TaskManagement.API.Interfaces;

namespace TaskManagement.API.Controllers;

// DTO para receber o Payload
public record TaskInputModel(
    [Required(ErrorMessage = "O título é obrigatório.")] string Title, 
    string Description
);

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
    public async Task<IActionResult> CreateTask([FromBody] TaskInputModel input)
    {
        var task = await _service.CreateTaskAsync(input.Title, input.Description);
        return Ok(task); 
    }
}