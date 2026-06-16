using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultIdPNoServicesToAestheticConsultation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConsultId",
                table: "AestheticConsultations",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PNo",
                table: "AestheticConsultations",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Services",
                table: "AestheticConsultations",
                type: "varchar(2000)",
                unicode: false,
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsultId",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "PNo",
                table: "AestheticConsultations");

            migrationBuilder.DropColumn(
                name: "Services",
                table: "AestheticConsultations");
        }
    }
}
