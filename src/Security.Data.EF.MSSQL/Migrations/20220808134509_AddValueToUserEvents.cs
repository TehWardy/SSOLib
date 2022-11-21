using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Security.Data.EF.Migrations
{
    public partial class AddValueToUserEvents : Migration
    {
        // Commented out code here is deliberate as the distributed session caching sets this up for us.
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "UserEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "UserEvents",
                type: "nvarchar(max)",
                nullable: true);
            /*
            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiresAtTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SlidingExpirationInSeconds = table.Column<long>(type: "bigint", nullable: false),
                    AbsoluteExpiration = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                });
            */

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_SessionId",
                table: "UserEvents",
                column: "SessionId");

            /*
            migrationBuilder.AddForeignKey(
                name: "FK_UserEvents_Sessions_SessionId",
                table: "UserEvents",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");
            */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(name: "FK_UserEvents_Sessions_SessionId", table: "UserEvents");
            //migrationBuilder.DropTable(name: "Sessions");

            migrationBuilder.DropIndex(name: "IX_UserEvents_SessionId", table: "UserEvents");
            migrationBuilder.DropColumn(name: "SessionId", table: "UserEvents");
            migrationBuilder.DropColumn(name: "Value", table: "UserEvents");
        }
    }
}