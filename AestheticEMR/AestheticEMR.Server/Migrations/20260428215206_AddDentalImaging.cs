using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddDentalImaging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DentalImagings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsultId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImagingType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToothRegion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Findings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Impression = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recommendations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentalImagings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DentalImagings");
        }
    }
}
