using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientSatisfactionSubmissionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PatientSatisfactionConsultId",
                table: "AestheticFollowUps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientSatisfactionPNo",
                table: "AestheticFollowUps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PatientSatisfactionSubmittedOn",
                table: "AestheticFollowUps",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientSatisfactionConsultId",
                table: "AestheticFollowUps");

            migrationBuilder.DropColumn(
                name: "PatientSatisfactionPNo",
                table: "AestheticFollowUps");

            migrationBuilder.DropColumn(
                name: "PatientSatisfactionSubmittedOn",
                table: "AestheticFollowUps");
        }
    }
}
