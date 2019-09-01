using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BallHogs.Migrations
{
    public partial class AddCustRost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Datum_BHTeams_BHTeamId",
                table: "Datum");

            migrationBuilder.DropIndex(
                name: "IX_Datum_BHTeamId",
                table: "Datum");

            migrationBuilder.DropColumn(
                name: "BHTeamId",
                table: "Datum");

            migrationBuilder.CreateTable(
                name: "PlayersOnTeams",
                columns: table => new
                {
                    PlayersOnTeamsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumId = table.Column<int>(nullable: false),
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
                    table.ForeignKey(
                        name: "FK_PlayersOnTeams_Datum_DatumId",
                        column: x => x.DatumId,
                        principalTable: "Datum",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayersOnTeams_BHTeamId",
                table: "PlayersOnTeams",
                column: "BHTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersOnTeams_DatumId",
                table: "PlayersOnTeams",
                column: "DatumId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayersOnTeams");

            migrationBuilder.AddColumn<int>(
                name: "BHTeamId",
                table: "Datum",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Datum_BHTeamId",
                table: "Datum",
                column: "BHTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Datum_BHTeams_BHTeamId",
                table: "Datum",
                column: "BHTeamId",
                principalTable: "BHTeams",
                principalColumn: "BHTeamId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
