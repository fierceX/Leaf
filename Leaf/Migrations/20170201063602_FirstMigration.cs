using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Leaf.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GapFillings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Answer = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Stems = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GapFillings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SingleChoices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Answer = table.Column<string>(nullable: true),
                    Choices1 = table.Column<string>(nullable: true),
                    Choices2 = table.Column<string>(nullable: true),
                    Choices3 = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Stems = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleChoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestPapers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BuildTime = table.Column<string>(nullable: true),
                    GapNum = table.Column<int>(nullable: false),
                    GapQuestionNum = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    SingleNum = table.Column<int>(nullable: false),
                    SingleQuestionNum = table.Column<string>(nullable: true),
                    Time = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestPapers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Admin = table.Column<int>(nullable: false),
                    BuildTime = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Score = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GapFillings");

            migrationBuilder.DropTable(
                name: "SingleChoices");

            migrationBuilder.DropTable(
                name: "TestPapers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
