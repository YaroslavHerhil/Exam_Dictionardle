using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exam_Dictionardle.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordsInfo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pronunciation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudioUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordsInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tries = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Miliseconds = table.Column<int>(type: "int", nullable: false),
                    WordID = table.Column<int>(type: "int", nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Games_WordsInfo_WordID",
                        column: x => x.WordID,
                        principalTable: "WordsInfo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_WordID",
                table: "Games",
                column: "WordID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "WordsInfo");
        }
    }
}
