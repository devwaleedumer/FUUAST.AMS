using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.DATA.Migrations
{
    /// <inheritdoc />
    public partial class correcting_Appform_aplicant_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationForm_Applicant",
                schema: "Domain",
                table: "Applicant");

            migrationBuilder.DropIndex(
                name: "IX_Applicant_ApplicationUserId",
                schema: "Domain",
                table: "Applicant");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationForm_ApplicantId",
                schema: "Domain",
                table: "ApplicationForm",
                column: "ApplicantId",
                unique: true,
                filter: "[ApplicantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Applicant_ApplicationUserId",
                schema: "Domain",
                table: "Applicant",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicant_ApplicationForm",
                schema: "Domain",
                table: "ApplicationForm",
                column: "ApplicantId",
                principalSchema: "Domain",
                principalTable: "Applicant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicant_ApplicationForm",
                schema: "Domain",
                table: "ApplicationForm");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationForm_ApplicantId",
                schema: "Domain",
                table: "ApplicationForm");

            migrationBuilder.DropIndex(
                name: "IX_Applicant_ApplicationUserId",
                schema: "Domain",
                table: "Applicant");

            migrationBuilder.CreateIndex(
                name: "IX_Applicant_ApplicationUserId",
                schema: "Domain",
                table: "Applicant",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationForm_Applicant",
                schema: "Domain",
                table: "Applicant",
                column: "ApplicationUserId",
                principalSchema: "Domain",
                principalTable: "ApplicationForm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
