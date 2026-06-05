using AestheticEMR.Core.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260605003000_FixMissingDentalTreatJsonColumns")]
    public partial class FixMissingDentalTreatJsonColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF COL_LENGTH('dbo.hDentalTreat', 'teethStatusJson') IS NULL
                    ALTER TABLE [dbo].[hDentalTreat] ADD [teethStatusJson] nvarchar(max) NULL;

                IF COL_LENGTH('dbo.hDentalTreat', 'orthodonticsJson') IS NULL
                    ALTER TABLE [dbo].[hDentalTreat] ADD [orthodonticsJson] nvarchar(max) NULL;

                IF COL_LENGTH('dbo.hDentalTreat', 'oralExamJson') IS NULL
                    ALTER TABLE [dbo].[hDentalTreat] ADD [oralExamJson] nvarchar(max) NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF COL_LENGTH('dbo.hDentalTreat', 'oralExamJson') IS NOT NULL
                    ALTER TABLE [dbo].[hDentalTreat] DROP COLUMN [oralExamJson];

                IF COL_LENGTH('dbo.hDentalTreat', 'orthodonticsJson') IS NOT NULL
                    ALTER TABLE [dbo].[hDentalTreat] DROP COLUMN [orthodonticsJson];

                IF COL_LENGTH('dbo.hDentalTreat', 'teethStatusJson') IS NOT NULL
                    ALTER TABLE [dbo].[hDentalTreat] DROP COLUMN [teethStatusJson];
                """);
        }
    }
}
