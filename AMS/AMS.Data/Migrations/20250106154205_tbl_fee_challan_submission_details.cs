using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.DATA.Migrations
{
    /// <inheritdoc />
    public partial class tbl_fee_challan_submission_details : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchCode",
                schema: "Domain",
                table: "FeeChallanSubmissionDetail");

            migrationBuilder.DropColumn(
                name: "BranchNameWithCity",
                schema: "Domain",
                table: "FeeChallanSubmissionDetail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchCode",
                schema: "Domain",
                table: "FeeChallanSubmissionDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BranchNameWithCity",
                schema: "Domain",
                table: "FeeChallanSubmissionDetail",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
