using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TaskManagement.API.Interfaces;
using TaskManagement.API.Services;
using TaskManagement.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Registra os serviços necessários para os Controllers funcionarem
builder.Services.AddControllers();

builder.Services.AddScoped<ITaskService, TaskService>(); // Adiciona o Liveness check diretamente na API
builder.Services.AddObservability();

builder.Services.AddHealthChecks()
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(), tags: new[] { "live" });
builder.Services.AddOpenApi();

var app = builder.Build();

// Configura o endpoint de Liveness (Vivacidade)
// Retorna 200 OK imediatamente se a aplicação não estiver travada.
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live")
});

// Configura o endpoint de Readiness (Prontidão)
// Só retorna 200 OK se todas as dependências com a tag "ready" estiverem ok.
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse // Formata a saída em um JSON amigável
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Mapeia as rotas configuradas nos atributos [Route] dos Controllers
app.MapControllers();

app.Run();

public partial class Program { }