using System;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagement.API.Services;
using Xunit;

namespace TaskManagement.UnitTests.Services;

public class TaskServiceTests
{
    private readonly Mock<ILogger<TaskService>> _mockLogger;
    private readonly Mock<IMeterFactory> _mockMeterFactory;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _mockLogger = new Mock<ILogger<TaskService>>();
        _mockMeterFactory = new Mock<IMeterFactory>();

        // Simula a criação do Meter para evitar NullReferenceException no construtor
        _mockMeterFactory
            .Setup(m => m.Create(It.IsAny<MeterOptions>()))
            .Returns(new Meter("TestMeter"));

        _service = new TaskService(_mockLogger.Object, _mockMeterFactory.Object);
    }

    [Fact]
    public async Task CreateTaskAsync_SemTitulo_LancaArgumentException()
    {
        // Arrange
        var tituloInvalido = "";
        var descricao = "Descrição de teste";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CreateTaskAsync(tituloInvalido, descricao)
        );

        Assert.Equal("O título não pode ser nulo ou vazio.", exception.Message);
    }
}