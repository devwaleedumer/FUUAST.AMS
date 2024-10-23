using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.DATA.Migrations
{
    /// <inheritdoc />
    public partial class program_department_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProgramDepartment_Departments",
                schema: "Lookup",
                table: "ProgramDepartment");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                schema: "Lookup",
                table: "ProgramDepartment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FaculityId",
                schema: "Lookup",
                table: "ProgramDepartment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramDepartment_FaculityId",
                schema: "Lookup",
                table: "ProgramDepartment",
                column: "FaculityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramDepartment_Departments",
                schema: "Lookup",
                table: "ProgramDepartment",
                column: "DepartmentId",
                principalSchema: "Lookup",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramDepartment_Faculty",
                schema: "Lookup",
                table: "ProgramDepartment",
                column: "FaculityId",
                principalSchema: "Lookup",
                principalTable: "Faculity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProgramDepartment_Departments",
                schema: "Lookup",
                table: "ProgramDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgramDepartment_Faculty",
                schema: "Lookup",
                table: "ProgramDepartment");

            migrationBuilder.DropIndex(
                name: "IX_ProgramDepartment_FaculityId",
                schema: "Lookup",
                table: "ProgramDepartment");

            migrationBuilder.DropColumn(
                name: "FaculityId",
                schema: "Lookup",
                table: "ProgramDepartment");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                schema: "Lookup",
                table: "ProgramDepartment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramDepartment_Departments",
                schema: "Lookup",
                table: "ProgramDepartment",
                column: "DepartmentId",
                principalSchema: "Lookup",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
