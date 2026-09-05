using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using TaskManagement.API.Diagnostics;

namespace TaskManagement.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddObservability(this IServiceCollection services)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(TelemetryConstants.ServiceName); 

        // Configuração do OpenTelemetry
        services.AddOpenTelemetry() 
            .WithTracing(tracerProviderBuilder => 
            {
                tracerProviderBuilder
                    .SetResourceBuilder(resourceBuilder) 
                    .AddAspNetCoreInstrumentation() // Captura requisições HTTP de entrada  
                    .AddHttpClientInstrumentation() // Captura requisições HTTP de saída  
                    .AddSource(TelemetryConstants.ServiceName) // Assina nossos traces customizados  
                    .AddConsoleExporter(); // Exporta para o console (útil para debug local)  
            })
            .WithMetrics(meterProviderBuilder =>  
            {
                meterProviderBuilder
                    .SetResourceBuilder(resourceBuilder)  
                    .AddAspNetCoreInstrumentation() // Métricas padrão do ASP.NET Core  
                    .AddHttpClientInstrumentation()  
                    .AddMeter(TelemetryConstants.MeterName) // Assina nossas métricas customizadas  
                    .AddConsoleExporter(); // Exporta para o console  
            });

        return services;
    }
}