using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebXemPhim.Migrations
{
    /// <inheritdoc />
    public partial class initialannn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TicketStatusesId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TicketStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketStatusesId",
                table: "Tickets",
                column: "TicketStatusesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketStatuses_TicketStatusesId",
                table: "Tickets",
                column: "TicketStatusesId",
                principalTable: "TicketStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketStatuses_TicketStatusesId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "TicketStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TicketStatusesId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketStatusesId",
                table: "Tickets");
        }
    }
}
