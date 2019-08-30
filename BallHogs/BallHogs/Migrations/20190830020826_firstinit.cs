using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BallHogs.Migrations
{
    public partial class firstinit : Migration
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
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Abbreviation = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Conference = table.Column<string>(nullable: true),
                    Division = table.Column<string>(nullable: true),
                    Full_name = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
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
                name: "Datum",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    First_name = table.Column<string>(nullable: true),
                    Height_feet = table.Column<int>(nullable: true),
                    Height_inches = table.Column<int>(nullable: true),
                    Last_name = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    TeamId = table.Column<int>(nullable: true),
                    Weight_pounds = table.Column<int>(nullable: true),
                    BHTeamId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Datum_BHTeams_BHTeamId",
                        column: x => x.BHTeamId,
                        principalTable: "BHTeams",
                        principalColumn: "BHTeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Datum_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
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

            migrationBuilder.CreateIndex(
                name: "IX_BHTeams_ManagerID",
                table: "BHTeams",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_Datum_BHTeamId",
                table: "Datum",
                column: "BHTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Datum_TeamId",
                table: "Datum",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerTeams_BHTeamId",
                table: "ManagerTeams",
                column: "BHTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerTeams_ManagerId",
                table: "ManagerTeams",
                column: "ManagerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Datum");

            migrationBuilder.DropTable(
                name: "ManagerTeams");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "BHTeams");

            migrationBuilder.DropTable(
                name: "Managers");
        }
    }
}
