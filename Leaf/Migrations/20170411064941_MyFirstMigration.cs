using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Leaf.Migrations
{
    public partial class MyFirstMigration : Migration
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
                    ImgPath = table.Column<string>(nullable: true),
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
                    ImgPath = table.Column<string>(nullable: true),
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
                    Level = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    SingleNum = table.Column<int>(nullable: false),
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

            migrationBuilder.CreateTable(
                name: "GapTest",
                columns: table => new
                {
                    GapId = table.Column<int>(nullable: false),
                    TestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GapTest", x => new { x.GapId, x.TestId });
                    table.ForeignKey(
                        name: "FK_GapTest_GapFillings_GapId",
                        column: x => x.GapId,
                        principalTable: "GapFillings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GapTest_TestPapers_TestId",
                        column: x => x.TestId,
                        principalTable: "TestPapers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SingleTest",
                columns: table => new
                {
                    SingleId = table.Column<int>(nullable: false),
                    TestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleTest", x => new { x.SingleId, x.TestId });
                    table.ForeignKey(
                        name: "FK_SingleTest_SingleChoices_SingleId",
                        column: x => x.SingleId,
                        principalTable: "SingleChoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SingleTest_TestPapers_TestId",
                        column: x => x.TestId,
                        principalTable: "TestPapers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Score = table.Column<double>(nullable: false),
                    TestId = table.Column<int>(nullable: false),
                    Time = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    gapnum = table.Column<int>(nullable: false),
                    singnum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTest_TestPapers_TestId",
                        column: x => x.TestId,
                        principalTable: "TestPapers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTest_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GapTest_TestId",
                table: "GapTest",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleTest_TestId",
                table: "SingleTest",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTest_TestId",
                table: "UserTest",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTest_UserId",
                table: "UserTest",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GapTest");

            migrationBuilder.DropTable(
                name: "SingleTest");

            migrationBuilder.DropTable(
                name: "UserTest");

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
