using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    public partial class ProductStockReportAndPreviousUnits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmtPaid",
                table: "BillingDetails");

            migrationBuilder.DropColumn(
                name: "EntryDate",
                table: "BillingDetails");

            migrationBuilder.DropColumn(
                name: "EntryTime",
                table: "BillingDetails");

            migrationBuilder.DropColumn(
                name: "ReversedPair",
                table: "BillingDetails");

            migrationBuilder.DropColumn(
                name: "TranID",
                table: "BillingDetails");

            migrationBuilder.DropColumn(
                name: "AmountSigned",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "AmtBF",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "AppName",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "EntryDate",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "EntryTime",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "InvNo",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "consultDate",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "diagnosis",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "isSigned",
                table: "Billing");

            migrationBuilder.DropColumn(
                name: "profFee",
                table: "Billing");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "bDate",
                table: "Billing",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AmtPaid",
                table: "BillingDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryDate",
                table: "BillingDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryTime",
                table: "BillingDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReversedPair",
                table: "BillingDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranID",
                table: "BillingDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "bDate",
                table: "Billing",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountSigned",
                table: "Billing",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmtBF",
                table: "Billing",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppName",
                table: "Billing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "Billing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryDate",
                table: "Billing",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryTime",
                table: "Billing",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvNo",
                table: "Billing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "consultDate",
                table: "Billing",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "diagnosis",
                table: "Billing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isSigned",
                table: "Billing",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "profFee",
                table: "Billing",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
