using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace SchoolScheduler.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Slots",
                columns: table => new
                {
                    SlotId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slots", x => x.SlotId);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ActivityId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TeacherId = table.Column<int>(nullable: false),
                    SubjectId = table.Column<int>(nullable: false),
                    ClassGroupId = table.Column<int>(nullable: false),
                    RoomId = table.Column<int>(nullable: false),
                    SlotId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_Activities_ClassGroups_ClassGroupId",
                        column: x => x.ClassGroupId,
                        principalTable: "ClassGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activities_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activities_Slots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "Slots",
                        principalColumn: "SlotId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activities_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activities_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Slots",
                columns: new[] { "SlotId", "Name" },
                values: new object[,]
                {
                    { 1, "Mo 8:00-8:45" },
                    { 25, "Fr 12:50-13:55" },
                    { 26, "Mo 13:45-14:30" },
                    { 27, "Tu 13:45-14:30" },
                    { 28, "We 13:45-14:30" },
                    { 29, "Th 13:45-14:30" },
                    { 30, "Fr 13:45-14:30" },
                    { 31, "Mo 14:40-15:25" },
                    { 32, "Tu 14:40-15:25" },
                    { 33, "We 14:40-15:25" },
                    { 34, "Th 14:40-15:25" },
                    { 35, "Fr 14:40-15:25" },
                    { 36, "Mo 15:35-16:20" },
                    { 37, "Tu 15:35-16:20" },
                    { 38, "We 15:35-16:20" },
                    { 39, "Th 15:35-16:20" },
                    { 40, "Fr 15:35-16:20" },
                    { 41, "Mo 16:30-17:15" },
                    { 42, "Tu 16:30-17:15" },
                    { 43, "We 16:30-17:15" },
                    { 24, "Th 12:50-13:55" },
                    { 44, "Th 16:30-17:15" },
                    { 23, "We 12:50-13:55" },
                    { 21, "Mo 12:50-13:55" },
                    { 2, "Tu 8:00-8:45" },
                    { 3, "We 8:00-8:45" },
                    { 4, "Th 8:00-8:45" },
                    { 5, "Fr 8:00-8:45" },
                    { 6, "Mo 8:55-9:40" },
                    { 7, "Tu 8:55-9:40" },
                    { 8, "We 8:55-9:40" },
                    { 9, "Th 8:55-9:40" },
                    { 10, "Fr 8:55-9:40" },
                    { 11, "Mo 9:50-11:35" },
                    { 12, "Tu 9:50-11:35" },
                    { 13, "We 9:50-11:35" },
                    { 14, "Th 9:50-11:35" },
                    { 15, "Fr 9:50-11:35" },
                    { 16, "Mo 11:55-12:40" },
                    { 17, "Tu 11:55-12:40" },
                    { 18, "We 11:55-12:40" },
                    { 19, "Th 11:55-12:40" },
                    { 20, "Fr 11:55-12:40" },
                    { 22, "Tu 12:50-13:55" },
                    { 45, "Fr 16:30-17:15" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ClassGroupId",
                table: "Activities",
                column: "ClassGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_RoomId",
                table: "Activities",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_SlotId",
                table: "Activities",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_SubjectId",
                table: "Activities",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_TeacherId",
                table: "Activities",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "ClassGroups");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Slots");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
