using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebXemPhim.Migrations
{
    /// <inheritdoc />
    public partial class addformovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PriceTicket",
                table: "Tickets",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Caster",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsHot",
                table: "Movies",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Caster",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "IsHot",
                table: "Movies");

            migrationBuilder.AlterColumn<double>(
                name: "PriceTicket",
                table: "Tickets",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
