using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorPeliculas.Server.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cines_peliculas_PeliculaId",
                table: "Cines");

            migrationBuilder.DropIndex(
                name: "IX_Cines_PeliculaId",
                table: "Cines");

            migrationBuilder.DropColumn(
                name: "PeliculaId",
                table: "Cines");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9548891f-9332-4cef-9fcb-ea21ae8999ff",
                column: "ConcurrencyStamp",
                value: "defc56a2-214d-4b8a-bf78-b8c6239d8558");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95842b29-d857-4c10-825d-852636fd75b5",
                column: "ConcurrencyStamp",
                value: "c59fb43f-2426-4f52-9ea0-a6ee8b3e4632");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PeliculaId",
                table: "Cines",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9548891f-9332-4cef-9fcb-ea21ae8999ff",
                column: "ConcurrencyStamp",
                value: "4e99bc81-ce0f-4866-b8b9-a0a99f0f05eb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95842b29-d857-4c10-825d-852636fd75b5",
                column: "ConcurrencyStamp",
                value: "3ae62a2d-b2d8-499f-931d-04b013e4821b");

            migrationBuilder.CreateIndex(
                name: "IX_Cines_PeliculaId",
                table: "Cines",
                column: "PeliculaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cines_peliculas_PeliculaId",
                table: "Cines",
                column: "PeliculaId",
                principalTable: "peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
