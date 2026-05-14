using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientSatisfactionSubmissionFields_Auto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAuditLogs_AestheticConsultations_ConsultationId",
                table: "AppAuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AppAuditLogs_AestheticPatients_PatientId",
                table: "AppAuditLogs");

            migrationBuilder.CreateTable(
                name: "AestheticFollowUp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsultationId = table.Column<int>(type: "int", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAutoScheduled = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientSatisfactionScore = table.Column<int>(type: "int", nullable: true),
                    RepeatPhotosTaken = table.Column<bool>(type: "bit", nullable: false),
                    NextTreatmentRecommendation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientSatisfactionConsultId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientSatisfactionPNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientSatisfactionSubmittedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AestheticFollowUp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AestheticFollowUp_AestheticConsultations_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "AestheticConsultations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppProductBatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuantityReceived = table.Column<int>(type: "int", nullable: false),
                    QuantityRemaining = table.Column<int>(type: "int", nullable: false),
                    IsRecalled = table.Column<bool>(type: "bit", nullable: false),
                    RecalledOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecallReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProductBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppProductBatches_AppProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "AppProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppProcedureProductUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductBatchId = table.Column<int>(type: "int", nullable: false),
                    ConsultationId = table.Column<int>(type: "int", nullable: false),
                    ProcedureType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QuantityUsed = table.Column<int>(type: "int", nullable: false),
                    UsedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProcedureProductUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppProcedureProductUsages_AestheticConsultations_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "AestheticConsultations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppProcedureProductUsages_AppProductBatches_ProductBatchId",
                        column: x => x.ProductBatchId,
                        principalTable: "AppProductBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppProcedureProductUsages_AppProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "AppProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AestheticFollowUp_ConsultationId",
                table: "AestheticFollowUp",
                column: "ConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProcedureProductUsages_ConsultationId",
                table: "AppProcedureProductUsages",
                column: "ConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProcedureProductUsages_ProductBatchId",
                table: "AppProcedureProductUsages",
                column: "ProductBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProcedureProductUsages_ProductId",
                table: "AppProcedureProductUsages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductBatches_ProductId_BatchNumber",
                table: "AppProductBatches",
                columns: new[] { "ProductId", "BatchNumber" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppAuditLogs_AestheticConsultations_ConsultationId",
                table: "AppAuditLogs",
                column: "ConsultationId",
                principalTable: "AestheticConsultations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAuditLogs_AestheticPatients_PatientId",
                table: "AppAuditLogs",
                column: "PatientId",
                principalTable: "AestheticPatients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAuditLogs_AestheticConsultations_ConsultationId",
                table: "AppAuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AppAuditLogs_AestheticPatients_PatientId",
                table: "AppAuditLogs");

            migrationBuilder.DropTable(
                name: "AestheticFollowUp");

            migrationBuilder.DropTable(
                name: "AppProcedureProductUsages");

            migrationBuilder.DropTable(
                name: "AppProductBatches");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAuditLogs_AestheticConsultations_ConsultationId",
                table: "AppAuditLogs",
                column: "ConsultationId",
                principalTable: "AestheticConsultations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AppAuditLogs_AestheticPatients_PatientId",
                table: "AppAuditLogs",
                column: "PatientId",
                principalTable: "AestheticPatients",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
