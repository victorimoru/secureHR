using Microsoft.EntityFrameworkCore;
using SecureHR.API.Endpoints;
using SecureHR.API.Middleware;
using SecureHR.Application.Features.Employee.Command;
using SecureHR.Core.Repositories;
using SecureHR.Infrastructure.Data;
using SecureHR.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseSqlServer(connectionString));
builder.Services.AddScoped<IIdempotencyRepository, SqlIdempotencyRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(HireEmployeeCommandHandler).Assembly));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<CustomExceptionHandlerMiddleware>();
app.UseHttpsRedirection();

app.MapEmployeeEndpoints();

app.Run();
