using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialBillingDetailFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "SNO",
                table: "BillingDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "SNO",
                table: "BillingDetails",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 0L);
        }
    }
}
