using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.API.Controllers;
using TaskManagement.API.Entities;
using TaskManagement.API.Interfaces;
using Xunit;

namespace TaskManagement.UnitTests.Controllers;

public class TasksControllerTests
{
    private readonly Mock<ITaskService> _mockService; 
    private readonly TasksController _controller;  

    public TasksControllerTests()
    {
        // Setup inicial
        _mockService = new Mock<ITaskService>();  
        _controller = new TasksController(_mockService.Object);  
    }

   [Fact]
    public async Task GetAll_RetornaOkComListaDeTarefas()
    {
        // Arrange
        var tarefasEsperadas = new List<TaskItem> 
        { 
            new TaskItem { Id = Guid.NewGuid(), Title = "Tarefa 1" },
            new TaskItem { Id = Guid.NewGuid(), Title = "Tarefa 2" }
        };

        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(tarefasEsperadas);

        // Act
        var resultado = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(resultado);
        var tarefasRetornadas = Assert.IsAssignableFrom<IEnumerable<TaskItem>>(okResult.Value);
        Assert.Equal(2, tarefasRetornadas.Count());
    }
}