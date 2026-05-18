using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    public partial class FixOpenIddictColumns : Migration
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

            migrationBuilder.DropIndex(
                name: "IX_AppAuditLogs_ConsultationId",
                table: "AppAuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AppAuditLogs_PatientId",
                table: "AppAuditLogs");

            migrationBuilder.DropColumn(
                name: "ConsultationId",
                table: "AppAuditLogs");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "AppAuditLogs");

            migrationBuilder.RenameColumn(
                name: "ProcedureType",
                table: "AppAuditLogs",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "OpenIddictApplications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isSigned",
                table: "Billing",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "billNO",
                table: "Billing",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // migrationBuilder.AddColumn<DateOnly>(
            //     name: "bDate",
            //     table: "Billing",
            //     type: "date",
            //     nullable: false,
            //     defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "AppAuditLogs",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SourceIp",
                table: "AppAuditLogs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(45)",
                oldMaxLength: 45,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewedBy",
                table: "AppAuditLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PerformedBy",
                table: "AppAuditLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldNullable: true);

            // migrationBuilder.AlterColumn<long>(
            //     name: "Id",
            //     table: "AppAuditLogs",
            //     type: "bigint",
            //     nullable: false,
            //     oldClrType: typeof(int),
            //     oldType: "int")
            //     .Annotation("SqlServer:Identity", "1, 1")
            //     .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "TranCode",
                table: "AppAuditLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AppAuditLogs_TranCode",
                table: "AppAuditLogs",
                column: "TranCode");

            // Change Id column from int to bigint (long) with PK drop/recreate
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppAuditLogs",
                table: "AppAuditLogs");
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "AppAuditLogs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
            migrationBuilder.AddPrimaryKey(
                name: "PK_AppAuditLogs",
                table: "AppAuditLogs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppAuditLogs_TranCode",
                table: "AppAuditLogs");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "bDate",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "TranCode",
                table: "AppAuditLogs");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AppAuditLogs",
                newName: "ProcedureType");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "OpenIddictApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isSigned",
                table: "Billing",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "billNO",
                table: "Billing",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "AppAuditLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SourceIp",
                table: "AppAuditLogs",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewedBy",
                table: "AppAuditLogs",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PerformedBy",
                table: "AppAuditLogs",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AppAuditLogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ConsultationId",
                table: "AppAuditLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "AppAuditLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppAuditLogs_ConsultationId",
                table: "AppAuditLogs",
                column: "ConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAuditLogs_PatientId",
                table: "AppAuditLogs",
                column: "PatientId");

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
    }
}
