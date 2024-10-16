using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.DATA.Migrations
{
    /// <inheritdoc />
    public partial class AddingHearAboutUniAndExpelledFromUni : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HeardAboutUniFrom",
                schema: "Domain",
                table: "Applicant",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "ExpelledFromUni",
                schema: "Domain",
                table: "Applicant",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpelledFromUni",
                schema: "Domain",
                table: "Applicant");

            migrationBuilder.AlterColumn<string>(
                name: "HeardAboutUniFrom",
                schema: "Domain",
                table: "Applicant",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
