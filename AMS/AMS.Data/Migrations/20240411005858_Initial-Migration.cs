using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.DATA.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Lookup");

            migrationBuilder.EnsureSchema(
                name: "Domain");

            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.CreateTable(
                name: "AcademicYear",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYear", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audit",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DegreeLevel",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DegreeLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgramType",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreviousDegreeDetail",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DegreeLevelId = table.Column<int>(type: "int", nullable: false),
                    DegreeName = table.Column<int>(type: "int", unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousDegreeDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DegreeLevel_PreviousDegreeDetails",
                        column: x => x.DegreeLevelId,
                        principalSchema: "Lookup",
                        principalTable: "DegreeLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Program",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ProgramCode = table.Column<int>(type: "int", nullable: false),
                    ProgramTypeId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    IsProgramOffered = table.Column<bool>(type: "bit", nullable: false),
                    Duration = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    ShiftEid = table.Column<int>(type: "int", nullable: false),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Program", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Department_Program",
                        column: x => x.DepartmentId,
                        principalSchema: "Lookup",
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgramType_Programs",
                        column: x => x.ProgramTypeId,
                        principalSchema: "Lookup",
                        principalTable: "ProgramType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleClaim",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Role",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applicant",
                schema: "Domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CNIC = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    DisablitityDetails = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WhatsappNo = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    NextOfKinName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    NextOfKinRelation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DomicileDistrict = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DomicileProvince = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    Religion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    BloodGroup = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applicant_User_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Auth",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                schema: "Auth",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "Auth",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                schema: "Auth",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdmissionSession",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcademicYearId = table.Column<int>(type: "int", nullable: true),
                    ProgramId = table.Column<int>(type: "int", nullable: true),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdmissionSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicYear_AdmissionSession",
                        column: x => x.AcademicYearId,
                        principalSchema: "Lookup",
                        principalTable: "AcademicYear",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdmissionSession_Program_ProgramId",
                        column: x => x.ProgramId,
                        principalSchema: "Lookup",
                        principalTable: "Program",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "Domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StreetAddress = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Province = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    District = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PostalCode = table.Column<int>(type: "int", nullable: false),
                    AddressTypeEid = table.Column<int>(type: "int", nullable: false),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Applicant",
                        column: x => x.ApplicantId,
                        principalSchema: "Domain",
                        principalTable: "Applicant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantDegree",
                schema: "Domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstituteName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    BorardOrUniversityName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    FromYear = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToYear = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MajorSubject = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    RollNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GradingTypeEid = table.Column<int>(type: "int", nullable: false),
                    ExamTypeEid = table.Column<int>(type: "int", nullable: false),
                    TranscriptUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalMarks = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    ObtainedMarks = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    ApplicantId = table.Column<int>(type: "int", nullable: true),
                    DegreeDetailId = table.Column<int>(type: "int", nullable: true),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantDegree", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantDegree_PreviousDegreeDetail_DegreeDetailId",
                        column: x => x.DegreeDetailId,
                        principalSchema: "Lookup",
                        principalTable: "PreviousDegreeDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Degree_Applicant",
                        column: x => x.ApplicantId,
                        principalSchema: "Domain",
                        principalTable: "Applicant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyContact",
                schema: "Domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactNO = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Relation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyContact", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_Applicant_EmergencyContact",
                        column: x => x.ApplicantId,
                        principalSchema: "Domain",
                        principalTable: "Applicant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Guardian",
                schema: "Domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Occupation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Relation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    TotalPerMonthIncome = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalPerMonthExpenses = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PhoneNo = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guardian", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_Applicant_Guardian",
                        column: x => x.ApplicantId,
                        principalSchema: "Domain",
                        principalTable: "Applicant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParentInfo",
                schema: "Domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FatherName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    MotherName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    FatherContact = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    FatherOccupation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    FatherCNIC = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    IsFatherDeceased = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentInfo", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_Applicant_ParentInfo",
                        column: x => x.ApplicantId,
                        principalSchema: "Domain",
                        principalTable: "Applicant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationForm",
                schema: "Domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    ApplicantId = table.Column<int>(type: "int", nullable: true),
                    InfoConsent = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    StatusEid = table.Column<int>(type: "int", nullable: true),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applicant_ApplicationForm",
                        column: x => x.ApplicantId,
                        principalSchema: "Domain",
                        principalTable: "Applicant",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SessionApplicationForms",
                        column: x => x.SessionId,
                        principalSchema: "Lookup",
                        principalTable: "AdmissionSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeeChallan",
                schema: "Domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoOfProgramsApplied = table.Column<int>(type: "int", nullable: false),
                    TotalFee = table.Column<int>(type: "int", nullable: false),
                    IssuedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueTill = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationFormId = table.Column<int>(type: "int", nullable: false),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeChallan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationForm_FeeChallan",
                        column: x => x.ApplicationFormId,
                        principalSchema: "Domain",
                        principalTable: "ApplicationForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgramApplied",
                schema: "Domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationFormId = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    PriorityEid = table.Column<int>(type: "int", nullable: false),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramApplied", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramApplied_ApplicationForm",
                        column: x => x.ApplicationFormId,
                        principalSchema: "Domain",
                        principalTable: "ApplicationForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgramApplied_Program",
                        column: x => x.ProgramId,
                        principalSchema: "Lookup",
                        principalTable: "Program",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeeChallanSubmissionDetail",
                schema: "Domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchCode = table.Column<int>(type: "int", nullable: false),
                    BranchNameWithCity = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    FeeChallanId = table.Column<int>(type: "int", nullable: false),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    InsertedBy = table.Column<int>(type: "int", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeChallanSubmissionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeChallan_FeeChallanSubmissionDetail",
                        column: x => x.FeeChallanId,
                        principalSchema: "Domain",
                        principalTable: "FeeChallan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_ApplicantId",
                schema: "Domain",
                table: "Address",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionSession_AcademicYearId",
                schema: "Lookup",
                table: "AdmissionSession",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionSession_ProgramId",
                schema: "Lookup",
                table: "AdmissionSession",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicant_ApplicationUserId",
                schema: "Domain",
                table: "Applicant",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantDegree_ApplicantId",
                schema: "Domain",
                table: "ApplicantDegree",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantDegree_DegreeDetailId",
                schema: "Domain",
                table: "ApplicantDegree",
                column: "DegreeDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationForm_ApplicantId",
                schema: "Domain",
                table: "ApplicationForm",
                column: "ApplicantId",
                unique: true,
                filter: "[ApplicantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationForm_SessionId",
                schema: "Domain",
                table: "ApplicationForm",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContact_ApplicantId",
                schema: "Domain",
                table: "EmergencyContact",
                column: "ApplicantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeeChallan_ApplicationFormId",
                schema: "Domain",
                table: "FeeChallan",
                column: "ApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeeChallanSubmissionDetail_FeeChallanId",
                schema: "Domain",
                table: "FeeChallanSubmissionDetail",
                column: "FeeChallanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guardian_ApplicantId",
                schema: "Domain",
                table: "Guardian",
                column: "ApplicantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParentInfo_ApplicantId",
                schema: "Domain",
                table: "ParentInfo",
                column: "ApplicantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreviousDegreeDetail_DegreeLevelId",
                schema: "Lookup",
                table: "PreviousDegreeDetail",
                column: "DegreeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Program_DepartmentId",
                schema: "Lookup",
                table: "Program",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Program_ProgramTypeId",
                schema: "Lookup",
                table: "Program",
                column: "ProgramTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramApplied_ApplicationFormId",
                schema: "Domain",
                table: "ProgramApplied",
                column: "ApplicationFormId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramApplied_ProgramId",
                schema: "Domain",
                table: "ProgramApplied",
                column: "ProgramId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Auth",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Auth",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Auth",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                schema: "Auth",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                schema: "Auth",
                table: "UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "Auth",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleClaim_RoleId",
                schema: "Auth",
                table: "UserRoleClaim",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "ApplicantDegree",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "Audit",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "EmergencyContact",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "FeeChallanSubmissionDetail",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "Guardian",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "ParentInfo",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "ProgramApplied",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "UserClaim",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserLogin",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserRoleClaim",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserToken",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "PreviousDegreeDetail",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "FeeChallan",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "DegreeLevel",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "ApplicationForm",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "Applicant",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "AdmissionSession",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "AcademicYear",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "Program",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "Department",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "ProgramType",
                schema: "Lookup");
        }
    }
}
