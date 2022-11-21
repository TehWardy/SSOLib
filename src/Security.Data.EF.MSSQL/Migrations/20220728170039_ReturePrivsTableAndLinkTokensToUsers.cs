using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Security.Data.EF.Migrations
{
    public partial class ReturePrivsTableAndLinkTokensToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Privileges");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Tokens",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_UserName",
                table: "Tokens",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Users_UserName",
                table: "Tokens",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Users_UserName",
                table: "Tokens");

            migrationBuilder.DropIndex(
                name: "IX_Tokens_UserName",
                table: "Tokens");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Tokens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Privileges",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PortalAdminsOnly = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privileges", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Privileges",
                columns: new[] { "Id", "Description", "Operation", "PortalAdminsOnly", "Type" },
                values: new object[,]
                {
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
        }
    }
}