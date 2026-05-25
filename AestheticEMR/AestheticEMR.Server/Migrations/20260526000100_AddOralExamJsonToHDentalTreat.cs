using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddOralExamJsonToHDentalTreat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "oralExamJson",
                table: "hDentalTreat",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "oralExamJson",
                table: "hDentalTreat");
        }
    }
}
