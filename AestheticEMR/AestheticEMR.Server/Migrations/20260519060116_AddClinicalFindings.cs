using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddClinicalFindings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InflammationOfGingiva",
                table: "hDentalTreat",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherClinicalFindings",
                table: "hDentalTreat",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresenceOfCalculus",
                table: "hDentalTreat",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresenceOfDebris",
                table: "hDentalTreat",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresenceOfStains",
                table: "hDentalTreat",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnderOrthodonticTreatment",
                table: "hDentalTreat",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InflammationOfGingiva",
                table: "hDentalTreat");

            migrationBuilder.DropColumn(
                name: "OtherClinicalFindings",
                table: "hDentalTreat");

            migrationBuilder.DropColumn(
                name: "PresenceOfCalculus",
                table: "hDentalTreat");

            migrationBuilder.DropColumn(
                name: "PresenceOfDebris",
                table: "hDentalTreat");

            migrationBuilder.DropColumn(
                name: "PresenceOfStains",
                table: "hDentalTreat");

            migrationBuilder.DropColumn(
                name: "UnderOrthodonticTreatment",
                table: "hDentalTreat");
        }
    }
}
