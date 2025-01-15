using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class TeamToLap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "team_id",
                table: "lap",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "team_id",
                table: "lap");
        }
    }
}
