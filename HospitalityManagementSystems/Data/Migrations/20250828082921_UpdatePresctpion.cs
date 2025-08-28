using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalityManagementSystems.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePresctpion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescription_MedicalRecord_MedicalRecordId",
                table: "Prescription");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescription_MedicalRecord_MedicalRecordId",
                table: "Prescription",
                column: "MedicalRecordId",
                principalTable: "MedicalRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescription_MedicalRecord_MedicalRecordId",
                table: "Prescription");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescription_MedicalRecord_MedicalRecordId",
                table: "Prescription",
                column: "MedicalRecordId",
                principalTable: "MedicalRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
