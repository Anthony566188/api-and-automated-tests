using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TaskManagement.IntegrationTests.Controllers;

public class TasksIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TasksIntegrationTests(WebApplicationFactory<Program> factory)
    {
        // Cria um cliente HTTP que se comunica com a API em memória
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_CriarTarefa_ComSucesso_RetornaOk()
    {
        // Arrange
        var payload = new { Title = "Estudar Testes de Integração", Description = "Usando WebApplicationFactory" };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", payload);

        // Assert
        // Valida se o status code é 200 OK (conforme a implementação atual do controller)
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); 
    }

    [Fact]
    public async Task Get_ListarTarefas_ComSucesso_RetornaOk()
    {
        // Arrange (garante que há pelo menos uma tarefa para listar)
        var payload = new { Title = "Tarefa para Listagem", Description = "Desc" };
        await _client.PostAsJsonAsync("/tasks", payload);

        // Act
        var response = await _client.GetAsync("/tasks");

        // Assert
        // Valida se a API responde corretamente ao GET
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Post_CriarTarefaSemTitulo_TratamentoDeErros_RetornaBadRequest()
    {
        // Arrange
        // Payload inválido: Título vazio/nulo
        var payloadInvalido = new { Title = "", Description = "Falta o título" };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", payloadInvalido);

        // Assert
        // Valida se a API responde com 400 Bad Request ao invés de estourar exceção 500
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}