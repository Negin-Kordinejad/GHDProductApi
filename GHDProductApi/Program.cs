using GHDProductApi.Core;
using GHDProductApi.Middlewares;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks().AddSqlServer(connectionString: builder.Configuration.GetConnectionString("SqlConnection"),
            healthQuery: "SELECT 1;",
            name: "sql",
            failureStatus: HealthStatus.Degraded,
            tags: ["db", "sql", "sqlserver"]);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(option =>
{
    option.AssumeDefaultVersionWhenUnspecified = true;
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product API",
        Version = "v1",
        Description = "CRUD opration for product",
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddCoreServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHealthChecks("/healthCheck");
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseGlobalErrorHandlingMiddleware();
app.UseLoggingMiddleWare();
app.MapControllers();


app.Run();

// Expose partial class for testing.
public partial class Program { }
