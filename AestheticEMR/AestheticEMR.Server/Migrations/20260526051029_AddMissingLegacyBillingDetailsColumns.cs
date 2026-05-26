using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingLegacyBillingDetailsColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
