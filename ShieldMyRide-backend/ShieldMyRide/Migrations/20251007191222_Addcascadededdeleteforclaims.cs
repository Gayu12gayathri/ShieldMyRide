using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShieldMyRide.Migrations
{
    /// <inheritdoc />
    public partial class Addcascadededdeleteforclaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsuranceClaims_Proposals_ProposalId",
                table: "InsuranceClaims");

            migrationBuilder.AddForeignKey(
                name: "FK_InsuranceClaims_Proposals_ProposalId",
                table: "InsuranceClaims",
                column: "ProposalId",
                principalTable: "Proposals",
                principalColumn: "ProposalId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsuranceClaims_Proposals_ProposalId",
                table: "InsuranceClaims");

            migrationBuilder.AddForeignKey(
                name: "FK_InsuranceClaims_Proposals_ProposalId",
                table: "InsuranceClaims",
                column: "ProposalId",
                principalTable: "Proposals",
                principalColumn: "ProposalId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
