using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    public partial class aestheticsUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "Pno",
                table: "AestheticPatients",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsultId",
                table: "AestheticPhotos");

            migrationBuilder.DropColumn(
                name: "PNo",
                table: "AestheticPhotos");

            migrationBuilder.DropColumn(
                name: "Pno",
                table: "AestheticPatients");
        }
    }
}
