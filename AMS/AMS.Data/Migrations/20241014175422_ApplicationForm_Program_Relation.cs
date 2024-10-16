using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.DATA.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationForm_Program_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProgramApplied_Programs",
                schema: "Domain",
                table: "ProgramApplied");

            migrationBuilder.DropIndex(
                name: "IX_ProgramApplied_ProgramId",
                schema: "Domain",
                table: "ProgramApplied");

            migrationBuilder.DropColumn(
                name: "ProgramId",
                schema: "Domain",
                table: "ProgramApplied");

            migrationBuilder.AlterColumn<int>(
                name: "SessionId",
                schema: "Domain",
                table: "ApplicationForm",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProgramId",
                schema: "Domain",
                table: "ApplicationForm",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationForm_ProgramId",
                schema: "Domain",
                table: "ApplicationForm",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationForms_Programs",
                schema: "Domain",
                table: "ApplicationForm",
                column: "ProgramId",
                principalSchema: "Lookup",
                principalTable: "Program",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationForms_Programs",
                schema: "Domain",
                table: "ApplicationForm");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationForm_ProgramId",
                schema: "Domain",
                table: "ApplicationForm");

            migrationBuilder.DropColumn(
                name: "ProgramId",
                schema: "Domain",
                table: "ApplicationForm");

            migrationBuilder.AddColumn<int>(
                name: "ProgramId",
                schema: "Domain",
                table: "ProgramApplied",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SessionId",
                schema: "Domain",
                table: "ApplicationForm",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramApplied_ProgramId",
                schema: "Domain",
                table: "ProgramApplied",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramApplied_Programs",
                schema: "Domain",
                table: "ProgramApplied",
                column: "ProgramId",
                principalSchema: "Lookup",
                principalTable: "Program",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
