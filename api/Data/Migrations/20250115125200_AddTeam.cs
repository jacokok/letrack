using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LeTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "team_id",
                table: "player",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "team",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_team", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_player_team_id",
                table: "player",
                column: "team_id");

            migrationBuilder.AddForeignKey(
                name: "fk_player_team_team_id",
                table: "player",
                column: "team_id",
                principalTable: "team",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_player_team_team_id",
                table: "player");

            migrationBuilder.DropTable(
                name: "team");

            migrationBuilder.DropIndex(
                name: "ix_player_team_id",
                table: "player");

            migrationBuilder.DropColumn(
                name: "team_id",
                table: "player");
        }
    }
}
