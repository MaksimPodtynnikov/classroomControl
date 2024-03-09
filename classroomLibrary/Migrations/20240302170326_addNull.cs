using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace classroomLibrary.Migrations
{
    /// <inheritdoc />
    public partial class addNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Enviroments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enviroments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Universities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Departments_City_cityId",
                        column: x => x.cityId,
                        principalTable: "City",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    direction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    faculty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    yearStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    yearEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    universityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.id);
                    table.ForeignKey(
                        name: "FK_Groups_Universities_universityId",
                        column: x => x.universityId,
                        principalTable: "Universities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    capacity = table.Column<int>(type: "int", nullable: false),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    departmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.id);
                    table.ForeignKey(
                        name: "FK_Classrooms_Departments_departmentId",
                        column: x => x.departmentId,
                        principalTable: "Departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    departmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Posts_Departments_departmentId",
                        column: x => x.departmentId,
                        principalTable: "Departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    telegram_id = table.Column<int>(type: "int", nullable: true),
                    groupId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    family = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    patronymic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.id);
                    table.ForeignKey(
                        name: "FK_Students_Groups_groupId",
                        column: x => x.groupId,
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassEnvironments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    count = table.Column<int>(type: "int", nullable: false),
                    classroomId = table.Column<int>(type: "int", nullable: false),
                    enviromentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassEnvironments", x => x.id);
                    table.ForeignKey(
                        name: "FK_ClassEnvironments_Classrooms_classroomId",
                        column: x => x.classroomId,
                        principalTable: "Classrooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassEnvironments_Enviroments_enviromentId",
                        column: x => x.enviromentId,
                        principalTable: "Enviroments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    oneC_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    postId = table.Column<int>(type: "int", nullable: false),
                    role = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    family = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    patronymic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Workers_Posts_postId",
                        column: x => x.postId,
                        principalTable: "Posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lessonNumber = table.Column<int>(type: "int", nullable: false),
                    workerId = table.Column<int>(type: "int", nullable: false),
                    classroomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.id);
                    table.ForeignKey(
                        name: "FK_Events_Classrooms_classroomId",
                        column: x => x.classroomId,
                        principalTable: "Classrooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_Workers_workerId",
                        column: x => x.workerId,
                        principalTable: "Workers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EventGroups",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    eventoId = table.Column<int>(type: "int", nullable: false),
                    groupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventGroups", x => x.id);
                    table.ForeignKey(
                        name: "FK_EventGroups_Events_eventoId",
                        column: x => x.eventoId,
                        principalTable: "Events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventGroups_Groups_groupId",
                        column: x => x.groupId,
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassEnvironments_classroomId",
                table: "ClassEnvironments",
                column: "classroomId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassEnvironments_enviromentId",
                table: "ClassEnvironments",
                column: "enviromentId");

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_departmentId",
                table: "Classrooms",
                column: "departmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_cityId",
                table: "Departments",
                column: "cityId");

            migrationBuilder.CreateIndex(
                name: "IX_EventGroups_eventoId",
                table: "EventGroups",
                column: "eventoId");

            migrationBuilder.CreateIndex(
                name: "IX_EventGroups_groupId",
                table: "EventGroups",
                column: "groupId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_classroomId",
                table: "Events",
                column: "classroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_workerId",
                table: "Events",
                column: "workerId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_universityId",
                table: "Groups",
                column: "universityId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_departmentId",
                table: "Posts",
                column: "departmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_groupId",
                table: "Students",
                column: "groupId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_postId",
                table: "Workers",
                column: "postId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassEnvironments");

            migrationBuilder.DropTable(
                name: "EventGroups");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Enviroments");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Universities");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "City");
        }
    }
}
