using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrderAPI.Application.DependencyInjection;
using OrderAPI.Infrastructure.DependencyInjection;
using OrderAPI.Infrastructure.Persistence;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRepositoryDependencyInjection();
builder.Services.AddServiceDependencyInjection();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Order Management API",
        Version = "v1",
        Description = @"API RESTful para gerenciamento de pedidos e ocorr�ncias.

        **Regras de Neg�cio:**
        - N�o � poss�vel cadastrar 2 ou mais ocorr�ncias do mesmo tipo em um intervalo de 10 minutos
        - A segunda ocorr�ncia de um pedido � automaticamente marcada como finalizadora
        - Ocorr�ncias do tipo 'SuccessfullyDelivered' marcam o pedido como entregue (IndEntregue = true)
        - N�o � poss�vel excluir ocorr�ncias de pedidos conclu�dos",
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Insira o token JWT no formato: Bearer {seu-token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });

    c.UseInlineDefinitionsForEnums();

    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
    foreach (var xml in xmlFiles)
    {
        c.IncludeXmlComments(xml, includeControllerXmlComments: true);
    }
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API v1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "Order API - Documenta��o";
    c.DisplayRequestDuration();
    c.EnableDeepLinking();
    c.EnableFilter();
});

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("Aplica��o iniciando...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplica��o finalizada inesperadamente!");
}
finally
{
    Log.CloseAndFlush();
}