using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShieldMyRide.Migrations
{
    /// <inheritdoc />
    public partial class proposalandquote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_Quotes_QuoteId",
                table: "Proposals");

            migrationBuilder.DropIndex(
                name: "IX_Proposals_QuoteId",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "QuoteId",
                table: "Proposals");

            migrationBuilder.AddColumn<int>(
                name: "ProposalId",
                table: "Quotes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_ProposalId",
                table: "Quotes",
                column: "ProposalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Proposals_ProposalId",
                table: "Quotes",
                column: "ProposalId",
                principalTable: "Proposals",
                principalColumn: "ProposalId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Proposals_ProposalId",
                table: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Quotes_ProposalId",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "ProposalId",
                table: "Quotes");

            migrationBuilder.AddColumn<int>(
                name: "QuoteId",
                table: "Proposals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_QuoteId",
                table: "Proposals",
                column: "QuoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_Quotes_QuoteId",
                table: "Proposals",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "QuoteId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
