using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorPeliculas.Server.Migrations
{
    public partial class adminuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CinePelicula");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "95842b29-d857-4c10-825d-852636fd75b5", "3ae62a2d-b2d8-499f-931d-04b013e4821b", null, null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9548891f-9332-4cef-9fcb-ea21ae8999ff", "95842b29-d857-4c10-825d-852636fd75b5" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cines_peliculas_PeliculaId",
                table: "Cines");

            migrationBuilder.DropIndex(
                name: "IX_Cines_PeliculaId",
                table: "Cines");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95842b29-d857-4c10-825d-852636fd75b5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9548891f-9332-4cef-9fcb-ea21ae8999ff", "95842b29-d857-4c10-825d-852636fd75b5" });

            migrationBuilder.DropColumn(
                name: "PeliculaId",
                table: "Cines");

            migrationBuilder.CreateTable(
                name: "CinePelicula",
                columns: table => new
                {
                    PeliculaId = table.Column<int>(type: "int", nullable: false),
                    cinesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinePelicula", x => new { x.PeliculaId, x.cinesId });
                    table.ForeignKey(
                        name: "FK_CinePelicula_Cines_cinesId",
                        column: x => x.cinesId,
                        principalTable: "Cines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CinePelicula_peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9548891f-9332-4cef-9fcb-ea21ae8999ff",
                column: "ConcurrencyStamp",
                value: "f4b96bbf-e939-43a4-93b3-bc8bf76b016b");

            migrationBuilder.CreateIndex(
                name: "IX_CinePelicula_cinesId",
                table: "CinePelicula",
                column: "cinesId");
        }
    }
}
