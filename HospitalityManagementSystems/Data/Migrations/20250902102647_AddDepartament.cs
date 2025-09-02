using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalityManagementSystems.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartamentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartamentId",
                table: "AspNetUsers");
        }
    }
}
