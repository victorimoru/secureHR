using MediatR;
using Microsoft.AspNetCore.Mvc;
using SecureHR.Application.Features.Employee.Command;

namespace SecureHR.API.Endpoints
{
    public static class EmployeeEndpoints
    {
        public static void MapEmployeeEndpoints(this IEndpointRouteBuilder app)
        {
            var employeeGroup = app.MapGroup("/api/employees").WithTags("Employees");

            employeeGroup.MapPost("/hire", static async ([FromHeader(Name = "Idempotency-Key")] Guid idempotencyKey,
                                           [FromBody] HireEmployeeCommand command,
                                           ISender sender) =>
            {
                var idempotentCommand = command with { IdempotencyKey = idempotencyKey };

                var employeeId = await sender.Send(idempotentCommand);

                if (employeeId == Guid.Empty)
                {
                    return Results.Conflict(new { Message = "This request has already been processed." });
                }

                return Results.Ok(new { EmployeeId = employeeId });
            })
            .WithName("HireEmployee")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);
        }
    }
}
