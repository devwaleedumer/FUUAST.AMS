using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.DATA.Migrations
{
    /// <inheritdoc />
    public partial class adding_verification_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubmitted",
                schema: "Domain",
                table: "FeeChallanSubmissionDetail");

            migrationBuilder.AddColumn<int>(
                name: "VerificationStatusEid",
                schema: "Domain",
                table: "ApplicationForm",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationStatusEid",
                schema: "Domain",
                table: "ApplicationForm");

            migrationBuilder.AddColumn<bool>(
                name: "IsSubmitted",
                schema: "Domain",
                table: "FeeChallanSubmissionDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
