using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exam_Dictionardle.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PlayerCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerName",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "PlayerID",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GamesPlayer = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlayerID",
                table: "Games",
                column: "PlayerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Player_PlayerID",
                table: "Games",
                column: "PlayerID",
                principalTable: "Player",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Player_PlayerID",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Games_PlayerID",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PlayerID",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "PlayerName",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
