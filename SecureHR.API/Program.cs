using Microsoft.EntityFrameworkCore;
using SecureHR.API.Endpoints;
using SecureHR.API.Middleware;
using SecureHR.Application;
using SecureHR.Application.Features.Employee.Command;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(HireEmployeeCommandHandler).Assembly));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CustomExceptionHandlerMiddleware>();
app.UseHttpsRedirection();

app.MapEmployeeEndpoints();

app.Run();
