using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    public partial class aestheticsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RisksAndComplications",
                table: "AestheticConsultations",
                type: "varchar(4000)",
                unicode: false,
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "AestheticConsultations",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostTreatmentInstructions",
                table: "AestheticConsultations",
                type: "varchar(4000)",
                unicode: false,
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceSettings",
                table: "AestheticConsultations",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AreaTreated",
                table: "AestheticConsultations",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrandUsed",
                table: "AestheticConsultations",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConsentDate",
                table: "AestheticConsultations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConsentNotes",
                table: "AestheticConsultations",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoolingMethod",
                table: "AestheticConsultations",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceUsed",
                table: "AestheticConsultations",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dilution",
                table: "AestheticConsultations",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fluence",
                table: "AestheticConsultations",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FollowUpReview",
                table: "AestheticConsultations",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Indication",
                table: "AestheticConsultations",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InjectionMapping",
                table: "AestheticConsultations",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotNumber",
                table: "AestheticConsultations",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextSessionDate",
                table: "AestheticConsultations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfShots",
                table: "AestheticConsultations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PulseDuration",
                table: "AestheticConsultations",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkinReaction",
                table: "AestheticConsultations",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpotSize",
                table: "AestheticConsultations",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitsUsed",
                table: "AestheticConsultations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Wavelength",
                table: "AestheticConsultations",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConsultId",
                table: "AestheticPhotos",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PNo",
                table: "AestheticPhotos",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaTreated",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "BrandUsed",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "ConsentDate",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "ConsentNotes",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "CoolingMethod",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "DeviceUsed",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "Dilution",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "Fluence",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "FollowUpReview",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "Indication",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "InjectionMapping",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "LotNumber",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "NextSessionDate",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "NumberOfShots",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "PulseDuration",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "SkinReaction",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "SpotSize",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "UnitsUsed",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "Wavelength",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "ConsultId",
                table: "AestheticPhotos");

            migrationBuilder.DropColumn(
                name: "PNo",
                table: "AestheticPhotos");

            migrationBuilder.AlterColumn<string>(
                name: "RisksAndComplications",
                table: "AestheticConsultations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldUnicode: false,
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "AestheticConsultations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostTreatmentInstructions",
                table: "AestheticConsultations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldUnicode: false,
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceSettings",
                table: "AestheticConsultations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
