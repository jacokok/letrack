using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class FKNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "team_id",
                table: "lap",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "race_id",
                table: "lap",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "player_id",
                table: "lap",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fk_lap_player_player_id",
                table: "lap",
                column: "player_id",
                principalTable: "player",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_lap_race_race_id",
                table: "lap",
                column: "race_id",
                principalTable: "race",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_lap_team_team_id",
                table: "lap",
                column: "team_id",
                principalTable: "team",
                principalColumn: "id");
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

            migrationBuilder.AlterColumn<int>(
                name: "team_id",
                table: "lap",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "race_id",
                table: "lap",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "player_id",
                table: "lap",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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
    }
}
