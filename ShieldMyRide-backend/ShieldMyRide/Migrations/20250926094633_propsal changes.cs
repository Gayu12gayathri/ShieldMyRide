using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShieldMyRide.Migrations
{
    /// <inheritdoc />
    public partial class propsalchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NCBPercent",
                table: "Proposals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "RoadsideAssist",
                table: "Proposals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ZeroDep",
                table: "Proposals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NCBPercent",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "RoadsideAssist",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "ZeroDep",
                table: "Proposals");
        }
    }
}
