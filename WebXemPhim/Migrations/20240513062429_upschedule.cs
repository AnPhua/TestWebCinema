using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebXemPhim.Migrations
{
    /// <inheritdoc />
    public partial class upschedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Schedules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Schedules",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
