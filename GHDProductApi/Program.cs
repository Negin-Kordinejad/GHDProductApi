using GHDProductApi.Core;
using GHDProductApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
