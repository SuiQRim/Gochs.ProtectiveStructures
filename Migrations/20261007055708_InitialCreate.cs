using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gochs.ProtectiveStructures.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProtectiveStructures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RegistrationNumber = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    Condition = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ResponsiblePerson = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectiveStructures", x => x.Id);
                    table.CheckConstraint("CK_ProtectiveStructures_Capacity_Positive", "\"Capacity\" > 0");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonnelNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Department = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Position = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ProtectiveStructureId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_ProtectiveStructures_ProtectiveStructureId",
                        column: x => x.ProtectiveStructureId,
                        principalTable: "ProtectiveStructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProtectiveStructureId = table.Column<int>(type: "INTEGER", nullable: false),
                    InspectionDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    InspectorName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ResultCondition = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Findings = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    RequiredActions = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    NextInspectionDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspections_ProtectiveStructures_ProtectiveStructureId",
                        column: x => x.ProtectiveStructureId,
                        principalTable: "ProtectiveStructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PersonnelNumber",
                table: "Employees",
                column: "PersonnelNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ProtectiveStructureId",
                table: "Employees",
                column: "ProtectiveStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_ProtectiveStructureId",
                table: "Inspections",
                column: "ProtectiveStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectiveStructures_RegistrationNumber",
                table: "ProtectiveStructures",
                column: "RegistrationNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.DropTable(
                name: "ProtectiveStructures");
        }
    }
}
