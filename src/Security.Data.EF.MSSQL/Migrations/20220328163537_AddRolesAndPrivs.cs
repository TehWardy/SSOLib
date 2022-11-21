using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Security.Data.EF.Migrations
{
    public partial class AddRolesAndPrivs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditDataItems_AuditEntries_AuditEntryId",
                schema: "Audit",
                table: "AuditDataItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LogDataItems_LogEntries_LogEntryId",
                schema: "Logging",
                table: "LogDataItems");

            migrationBuilder.CreateTable(
                name: "Privileges",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PortalAdminsOnly = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privileges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersArePortalAdmins = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Privs = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Privileges",
                columns: new[] { "Id", "Description", "Operation", "PortalAdminsOnly", "Type" },
                values: new object[,]
                {
                    { "auditdataitem_create", "Allows users to Create AuditDataItems.", "Create", false, "AuditDataItem" },
                    { "auditdataitem_delete", "Allows users to Delete AuditDataItems.", "Delete", false, "AuditDataItem" },
                    { "auditdataitem_read", "Allows users to Read AuditDataItems.", "Read", false, "AuditDataItem" },
                    { "auditdataitem_update", "Allows users to Update AuditDataItems.", "Update", false, "AuditDataItem" },
                    { "auditentry_create", "Allows users to Create AuditEntrys.", "Create", false, "AuditEntry" },
                    { "auditentry_delete", "Allows users to Delete AuditEntrys.", "Delete", false, "AuditEntry" },
                    { "auditentry_read", "Allows users to Read AuditEntrys.", "Read", false, "AuditEntry" },
                    { "auditentry_update", "Allows users to Update AuditEntrys.", "Update", false, "AuditEntry" },
                    { "logdataitem_create", "Allows users to Create LogDataItems.", "Create", false, "LogDataItem" },
                    { "logdataitem_delete", "Allows users to Delete LogDataItems.", "Delete", false, "LogDataItem" },
                    { "logdataitem_read", "Allows users to Read LogDataItems.", "Read", false, "LogDataItem" },
                    { "logdataitem_update", "Allows users to Update LogDataItems.", "Update", false, "LogDataItem" },
                    { "logentry_create", "Allows users to Create LogEntrys.", "Create", false, "LogEntry" },
                    { "logentry_delete", "Allows users to Delete LogEntrys.", "Delete", false, "LogEntry" },
                    { "logentry_read", "Allows users to Read LogEntrys.", "Read", false, "LogEntry" },
                    { "logentry_update", "Allows users to Update LogEntrys.", "Update", false, "LogEntry" },
                    { "ssoprivilege_create", "Allows users to Create SSOPrivileges.", "Create", false, "SSOPrivilege" },
                    { "ssoprivilege_delete", "Allows users to Delete SSOPrivileges.", "Delete", false, "SSOPrivilege" },
                    { "ssoprivilege_read", "Allows users to Read SSOPrivileges.", "Read", false, "SSOPrivilege" },
                    { "ssoprivilege_update", "Allows users to Update SSOPrivileges.", "Update", false, "SSOPrivilege" },
                    { "ssorole_create", "Allows users to Create SSORoles.", "Create", false, "SSORole" },
                    { "ssorole_delete", "Allows users to Delete SSORoles.", "Delete", false, "SSORole" },
                    { "ssorole_read", "Allows users to Read SSORoles.", "Read", false, "SSORole" },
                    { "ssorole_update", "Allows users to Update SSORoles.", "Update", false, "SSORole" },
                    { "ssouser_create", "Allows users to Create SSOUsers.", "Create", false, "SSOUser" },
                    { "ssouser_delete", "Allows users to Delete SSOUsers.", "Delete", false, "SSOUser" },
                    { "ssouser_read", "Allows users to Read SSOUsers.", "Read", false, "SSOUser" },
                    { "ssouser_update", "Allows users to Update SSOUsers.", "Update", false, "SSOUser" },
                    { "token_create", "Allows users to Create Tokens.", "Create", false, "Token" },
                    { "token_delete", "Allows users to Delete Tokens.", "Delete", false, "Token" },
                    { "token_read", "Allows users to Read Tokens.", "Read", false, "Token" },
                    { "token_update", "Allows users to Update Tokens.", "Update", false, "Token" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditDataItems_AuditEntries_AuditEntryId",
                schema: "Audit",
                table: "AuditDataItems",
                column: "AuditEntryId",
                principalSchema: "Audit",
                principalTable: "AuditEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LogDataItems_LogEntries_LogEntryId",
                schema: "Logging",
                table: "LogDataItems",
                column: "LogEntryId",
                principalSchema: "Logging",
                principalTable: "LogEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditDataItems_AuditEntries_AuditEntryId",
                schema: "Audit",
                table: "AuditDataItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LogDataItems_LogEntries_LogEntryId",
                schema: "Logging",
                table: "LogDataItems");

            migrationBuilder.DropTable(
                name: "Privileges");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditDataItems_AuditEntries_AuditEntryId",
                schema: "Audit",
                table: "AuditDataItems",
                column: "AuditEntryId",
                principalSchema: "Audit",
                principalTable: "AuditEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LogDataItems_LogEntries_LogEntryId",
                schema: "Logging",
                table: "LogDataItems",
                column: "LogEntryId",
                principalSchema: "Logging",
                principalTable: "LogEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
