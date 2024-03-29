﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Security.Data.EF.Migrations
{
    public partial class AddTokenStorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 64, nullable: false),
                    Reason = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Expires = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Tokens");
        }
    }
}