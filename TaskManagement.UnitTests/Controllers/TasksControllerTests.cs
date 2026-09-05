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
    public async Task GetById_TaskExiste_RetornaOk()  
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var taskEsperada = new TaskItem { Id = taskId, Title = "Aprender xUnit" };  

        // Simulando o retorno do serviço
        _mockService.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync(taskEsperada);  

        // Act
        var resultado = await _controller.GetById(taskId);  

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(resultado);  
        var taskRetornada = Assert.IsType<TaskItem>(okResult.Value);  
        Assert.Equal("Aprender xUnit", taskRetornada.Title);  
    }

    [Fact]  
    public async Task GetById_TaskNaoExiste_RetornaNotFound()  
    {
        // Arrange
        var taskId = Guid.NewGuid();

        // Simulando a ausência do registro
        _mockService.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync((TaskItem)null!);  

        // Act
        var resultado = await _controller.GetById(taskId);  

        // Assert
        Assert.IsType<NotFoundResult>(resultado);  
    }
}