using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.DATA.Migrations
{
    /// <inheritdoc />
    public partial class set_relations_changed_fields_sizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationForm_EntranceTestDetail_EntranceTestId",
                schema: "Domain",
                table: "ApplicationForm");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationForm_EntranceTestId",
                schema: "Domain",
                table: "ApplicationForm");

            migrationBuilder.DropColumn(
                name: "EntranceTestId",
                schema: "Domain",
                table: "ApplicationForm");

            migrationBuilder.DropColumn(
                name: "HaveValidTest",
                schema: "Domain",
                table: "ApplicationForm");

            migrationBuilder.DropColumn(
                name: "StatusEid",
                schema: "Domain",
                table: "ApplicationForm");

            migrationBuilder.AlterColumn<string>(
                name: "BoardOrUniversityName",
                schema: "Domain",
                table: "ApplicantDegree",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Religion",
                schema: "Domain",
                table: "Applicant",
                type: "varchar(30)",
                unicode: false,
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldUnicode: false,
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "FatherName",
                schema: "Domain",
                table: "Applicant",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(13)",
                oldUnicode: false,
                oldMaxLength: 13);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                schema: "Domain",
                table: "Applicant",
                type: "varchar(30)",
                unicode: false,
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntranceTestId",
                schema: "Domain",
                table: "ApplicationForm",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HaveValidTest",
                schema: "Domain",
                table: "ApplicationForm",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StatusEid",
                schema: "Domain",
                table: "ApplicationForm",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BoardOrUniversityName",
                schema: "Domain",
                table: "ApplicantDegree",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Religion",
                schema: "Domain",
                table: "Applicant",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldUnicode: false,
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "FatherName",
                schema: "Domain",
                table: "Applicant",
                type: "varchar(13)",
                unicode: false,
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                schema: "Domain",
                table: "Applicant",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldUnicode: false,
                oldMaxLength: 30);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationForm_EntranceTestId",
                schema: "Domain",
                table: "ApplicationForm",
                column: "EntranceTestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationForm_EntranceTestDetail_EntranceTestId",
                schema: "Domain",
                table: "ApplicationForm",
                column: "EntranceTestId",
                principalSchema: "Domain",
                principalTable: "EntranceTestDetail",
                principalColumn: "Id");
        }
    }
}
