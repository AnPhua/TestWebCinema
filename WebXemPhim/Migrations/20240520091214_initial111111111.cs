using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebXemPhim.Migrations
{
    /// <inheritdoc />
    public partial class initial111111111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketStatusId",
                table: "Tickets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketStatusId",
                table: "Tickets",
                type: "int",
                nullable: true);
        }
    }
}
