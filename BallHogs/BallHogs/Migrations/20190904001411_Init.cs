using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BallHogs.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    ManagerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true),
                    Wins = table.Column<int>(nullable: false),
                    Losses = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.ManagerID);
                });

            migrationBuilder.CreateTable(
                name: "BHTeams",
                columns: table => new
                {
                    BHTeamId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamName = table.Column<string>(nullable: true),
                    ManagerName = table.Column<string>(nullable: true),
                    ManagerID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BHTeams", x => x.BHTeamId);
                    table.ForeignKey(
                        name: "FK_BHTeams_Managers_ManagerID",
                        column: x => x.ManagerID,
                        principalTable: "Managers",
                        principalColumn: "ManagerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ManagerTeams",
                columns: table => new
                {
                    ManagerTeamId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ManagerId = table.Column<int>(nullable: false),
                    BHTeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerTeams", x => x.ManagerTeamId);
                    table.ForeignKey(
                        name: "FK_ManagerTeams_BHTeams_BHTeamId",
                        column: x => x.BHTeamId,
                        principalTable: "BHTeams",
                        principalColumn: "BHTeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManagerTeams_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "ManagerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayersOnTeams",
                columns: table => new
                {
                    PlayersOnTeamsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlayerAPINum = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    PPG = table.Column<float>(nullable: false),
                    Steals = table.Column<float>(nullable: false),
                    Rebounds = table.Column<float>(nullable: false),
                    BHTeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayersOnTeams", x => x.PlayersOnTeamsId);
                    table.ForeignKey(
                        name: "FK_PlayersOnTeams_BHTeams_BHTeamId",
                        column: x => x.BHTeamId,
                        principalTable: "BHTeams",
                        principalColumn: "BHTeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BHTeams_ManagerID",
                table: "BHTeams",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerTeams_BHTeamId",
                table: "ManagerTeams",
                column: "BHTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerTeams_ManagerId",
                table: "ManagerTeams",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersOnTeams_BHTeamId",
                table: "PlayersOnTeams",
                column: "BHTeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManagerTeams");

            migrationBuilder.DropTable(
                name: "PlayersOnTeams");

            migrationBuilder.DropTable(
                name: "BHTeams");

            migrationBuilder.DropTable(
                name: "Managers");
        }
    }
}
