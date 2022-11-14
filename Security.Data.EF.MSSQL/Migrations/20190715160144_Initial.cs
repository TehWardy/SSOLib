using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Security.Data.EF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Audit");

            migrationBuilder.EnsureSchema(
                name: "Logging");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEndDateUtc = table.Column<DateTime>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditEntries",
                schema: "Audit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EntityId = table.Column<string>(nullable: true),
                    EventId = table.Column<Guid>(nullable: false),
                    EntityType = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    Event = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    TimeOfEvent = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogEntries",
                schema: "Logging",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Level = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: false),
                    AppName = table.Column<string>(nullable: false),
                    TypeName = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditDataItems",
                schema: "Audit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuditEntryId = table.Column<int>(nullable: false),
                    PropertyName = table.Column<string>(nullable: true),
                    PreviousValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditDataItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditDataItems_AuditEntries_AuditEntryId",
                        column: x => x.AuditEntryId,
                        principalSchema: "Audit",
                        principalTable: "AuditEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogDataItems",
                schema: "Logging",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LogEntryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogDataItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogDataItems_LogEntries_LogEntryId",
                        column: x => x.LogEntryId,
                        principalSchema: "Logging",
                        principalTable: "LogEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditDataItems_AuditEntryId",
                schema: "Audit",
                table: "AuditDataItems",
                column: "AuditEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_LogDataItems_LogEntryId",
                schema: "Logging",
                table: "LogDataItems",
                column: "LogEntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AuditDataItems",
                schema: "Audit");

            migrationBuilder.DropTable(
                name: "LogDataItems",
                schema: "Logging");

            migrationBuilder.DropTable(
                name: "AuditEntries",
                schema: "Audit");

            migrationBuilder.DropTable(
                name: "LogEntries",
                schema: "Logging");
        }
    }
}
