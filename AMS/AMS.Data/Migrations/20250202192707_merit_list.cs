using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.DATA.Migrations
{
    /// <inheritdoc />
    public partial class merit_list : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                schema: "Domain",
                table: "ApplicationForm",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MeritList",
                schema: "Domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    MeritListNo = table.Column<int>(type: "int", nullable: true),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeritList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeritList_Department",
                        column: x => x.DepartmentId,
                        principalSchema: "Lookup",
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeritList_Program",
                        column: x => x.ProgramId,
                        principalSchema: "Lookup",
                        principalTable: "Program",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeritList_Session",
                        column: x => x.SessionId,
                        principalSchema: "Lookup",
                        principalTable: "AdmissionSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeritList_Shift",
                        column: x => x.ShiftId,
                        principalSchema: "Lookup",
                        principalTable: "TimeShift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeritListDetails",
                schema: "Domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeritListId = table.Column<int>(type: "int", nullable: false),
                    ApplicationFormId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeritListDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationForm_MeritListDetails",
                        column: x => x.ApplicationFormId,
                        principalSchema: "Domain",
                        principalTable: "ApplicationForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeritListDetails_MeritList",
                        column: x => x.MeritListId,
                        principalSchema: "Domain",
                        principalTable: "MeritList",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeritList_DepartmentId",
                schema: "Domain",
                table: "MeritList",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MeritList_ProgramId",
                schema: "Domain",
                table: "MeritList",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_MeritList_SessionId_ProgramId_DepartmentId_ShiftId",
                schema: "Domain",
                table: "MeritList",
                columns: new[] { "SessionId", "ProgramId", "DepartmentId", "ShiftId" });

            migrationBuilder.CreateIndex(
                name: "IX_MeritList_ShiftId",
                schema: "Domain",
                table: "MeritList",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_MeritListDetails_ApplicationFormId",
                schema: "Domain",
                table: "MeritListDetails",
                column: "ApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeritListDetails_MeritListId",
                schema: "Domain",
                table: "MeritListDetails",
                column: "MeritListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeritListDetails",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "MeritList",
                schema: "Domain");

            migrationBuilder.DropColumn(
                name: "Remarks",
                schema: "Domain",
                table: "ApplicationForm");
        }
    }
}
