using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace classroomLibrary.Migrations
{
    /// <inheritdoc />
    public partial class descl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Classrooms",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "Classrooms");
        }
    }
}
