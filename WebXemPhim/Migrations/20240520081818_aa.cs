using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebXemPhim.Migrations
{
    /// <inheritdoc />
    public partial class aa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketStatuses_TicketStatusesId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "TicketStatusesId",
                table: "Tickets",
                newName: "TicketStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_TicketStatusesId",
                table: "Tickets",
                newName: "IX_Tickets_TicketStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketStatuses_TicketStatusId",
                table: "Tickets",
                column: "TicketStatusId",
                principalTable: "TicketStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketStatuses_TicketStatusId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "TicketStatusId",
                table: "Tickets",
                newName: "TicketStatusesId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_TicketStatusId",
                table: "Tickets",
                newName: "IX_Tickets_TicketStatusesId");

            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketStatuses_TicketStatusesId",
                table: "Tickets",
                column: "TicketStatusesId",
                principalTable: "TicketStatuses",
                principalColumn: "Id");
        }
    }
}
