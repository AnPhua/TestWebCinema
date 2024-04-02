using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebXemPhim.Migrations
{
    /// <inheritdoc />
    public partial class ud4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_RankCustomers_RankCustomerId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserStatuses_UserStatusId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "UserStatusId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RankCustomerId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_RankCustomers_RankCustomerId",
                table: "Users",
                column: "RankCustomerId",
                principalTable: "RankCustomers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserStatuses_UserStatusId",
                table: "Users",
                column: "UserStatusId",
                principalTable: "UserStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_RankCustomers_RankCustomerId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserStatuses_UserStatusId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "UserStatusId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RankCustomerId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_RankCustomers_RankCustomerId",
                table: "Users",
                column: "RankCustomerId",
                principalTable: "RankCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserStatuses_UserStatusId",
                table: "Users",
                column: "UserStatusId",
                principalTable: "UserStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
