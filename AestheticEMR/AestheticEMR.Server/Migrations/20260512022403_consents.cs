using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    public partial class consents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppAestheticConsentTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProcedureType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAestheticConsentTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppAestheticSignedConsents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    ConsentTemplateId = table.Column<int>(type: "int", nullable: false),
                    ConsultId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProcedureType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SignedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    WitnessedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SignatureName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsentContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SignatureImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DoctorViewedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DoctorViewedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVoided = table.Column<bool>(type: "bit", nullable: false),
                    VoidReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAestheticSignedConsents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAestheticSignedConsents_AestheticPatients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AestheticPatients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AppAestheticSignedConsents_AppAestheticConsentTemplates_ConsentTemplateId",
                        column: x => x.ConsentTemplateId,
                        principalTable: "AppAestheticConsentTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppAestheticSignedConsents_ConsentTemplateId",
                table: "AppAestheticSignedConsents",
                column: "ConsentTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAestheticSignedConsents_ConsultId_ProcedureType_IsVoided",
                table: "AppAestheticSignedConsents",
                columns: new[] { "ConsultId", "ProcedureType", "IsVoided" });

            migrationBuilder.CreateIndex(
                name: "IX_AppAestheticSignedConsents_PatientId",
                table: "AppAestheticSignedConsents",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAestheticSignedConsents_PNo",
                table: "AppAestheticSignedConsents",
                column: "PNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppAestheticSignedConsents");

            migrationBuilder.DropTable(
                name: "AppAestheticConsentTemplates");
        }
    }
}
