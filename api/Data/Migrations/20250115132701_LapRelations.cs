using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class LapRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_lap_player_id",
                table: "lap",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_lap_race_id",
                table: "lap",
                column: "race_id");

            migrationBuilder.CreateIndex(
                name: "ix_lap_team_id",
                table: "lap",
                column: "team_id");

            migrationBuilder.AddForeignKey(
                name: "fk_lap_player_player_id",
                table: "lap",
                column: "player_id",
                principalTable: "player",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lap_race_race_id",
                table: "lap",
                column: "race_id",
                principalTable: "race",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lap_team_team_id",
                table: "lap",
                column: "team_id",
                principalTable: "team",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_lap_player_player_id",
                table: "lap");

            migrationBuilder.DropForeignKey(
                name: "fk_lap_race_race_id",
                table: "lap");

            migrationBuilder.DropForeignKey(
                name: "fk_lap_team_team_id",
                table: "lap");

            migrationBuilder.DropIndex(
                name: "ix_lap_player_id",
                table: "lap");

            migrationBuilder.DropIndex(
                name: "ix_lap_race_id",
                table: "lap");

            migrationBuilder.DropIndex(
                name: "ix_lap_team_id",
                table: "lap");
        }
    }
}
