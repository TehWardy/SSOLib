using Microsoft.EntityFrameworkCore.Migrations;

namespace Security.Data.EF.Migrations
{
    public partial class AddSystemUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DisplayName", "AccessFailedCount", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEndDateUtc", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed" },
                values: new object[,]
                {
                    { "system", "System", 0, "system@corporatelinx.com", true, false, null, "", "", true }
                }
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intentionally empty
        }
    }
}
