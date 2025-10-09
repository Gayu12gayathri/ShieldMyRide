using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShieldMyRide.Migrations
{
    /// <inheritdoc />
    public partial class updatedproposalandpayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Proposals",
                newName: "ProposalStatus");

            migrationBuilder.AddColumn<decimal>(
                name: "Premium",
                table: "Proposals",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Premium",
                table: "Proposals");

            migrationBuilder.RenameColumn(
                name: "ProposalStatus",
                table: "Proposals",
                newName: "Status");
        }
    }
}
