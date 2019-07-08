using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManager.Data.Migrations
{
    public partial class Third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Users_UserId",
                table: "ToDos");

            migrationBuilder.DropIndex(
                name: "IX_ToDos_UserId",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "ToDoStatus",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ToDos");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ToDos",
                newName: "Title");

            migrationBuilder.AddColumn<int>(
                name: "HandledBy",
                table: "ToDos",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "ToDos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ToDos_HandledBy",
                table: "ToDos",
                column: "HandledBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Users_HandledBy",
                table: "ToDos",
                column: "HandledBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Users_HandledBy",
                table: "ToDos");

            migrationBuilder.DropIndex(
                name: "IX_ToDos_HandledBy",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "HandledBy",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "ToDos");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "ToDos",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "ToDoStatus",
                table: "ToDos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ToDos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ToDos_UserId",
                table: "ToDos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Users_UserId",
                table: "ToDos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
