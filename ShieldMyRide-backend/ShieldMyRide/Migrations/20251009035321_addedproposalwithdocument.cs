using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShieldMyRide.Migrations
{
    /// <inheritdoc />
    public partial class addedproposalwithdocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressProofPath",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrivingLicensePath",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncomeProofPath",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportPhotoPath",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousInsurancePath",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignaturePath",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressProofPath",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "DrivingLicensePath",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "IncomeProofPath",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "PassportPhotoPath",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "PreviousInsurancePath",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "SignaturePath",
                table: "Proposals");
        }
    }
}
