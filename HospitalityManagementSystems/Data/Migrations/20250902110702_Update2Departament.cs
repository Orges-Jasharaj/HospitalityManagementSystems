using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalityManagementSystems.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update2Departament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Departaments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Departaments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Departaments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Departaments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Departaments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Departaments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Departaments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Departaments");
        }
    }
}
