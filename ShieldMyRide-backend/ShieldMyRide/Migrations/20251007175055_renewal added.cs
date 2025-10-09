using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShieldMyRide.Migrations
{
    /// <inheritdoc />
    public partial class renewaladded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRenewal",
                table: "Proposals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PolicyEndDate",
                table: "Proposals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PolicyStartDate",
                table: "Proposals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RenewalOfProposalId",
                table: "Proposals",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRenewal",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "PolicyEndDate",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "PolicyStartDate",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "RenewalOfProposalId",
                table: "Proposals");
        }
    }
}
