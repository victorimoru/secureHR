using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureHR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeEmployeeAggregrateOwnsSalaryAdjustmentAndLeaveBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SalaryAdjustments",
                table: "SalaryAdjustments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveBookings",
                table: "LeaveBookings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalaryAdjustments",
                table: "SalaryAdjustments",
                columns: new[] { "Id", "EmployeeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveBookings",
                table: "LeaveBookings",
                columns: new[] { "Id", "EmployeeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SalaryAdjustments",
                table: "SalaryAdjustments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveBookings",
                table: "LeaveBookings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalaryAdjustments",
                table: "SalaryAdjustments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveBookings",
                table: "LeaveBookings",
                column: "Id");
        }
    }
}
