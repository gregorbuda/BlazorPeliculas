using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorPeliculas.Server.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "genero",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genero", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "peliculas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Resumen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnCartelera = table.Column<bool>(type: "bit", nullable: false),
                    Trailer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lanzamiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Poster = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_peliculas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "persona",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Biografia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persona", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "generoPeliculas",
                columns: table => new
                {
                    PeliculaId = table.Column<int>(type: "int", nullable: false),
                    GeneroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_generoPeliculas", x => new { x.GeneroId, x.PeliculaId });
                    table.ForeignKey(
                        name: "FK_generoPeliculas_genero_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "genero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_generoPeliculas_peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "peliculaActor",
                columns: table => new
                {
                    PersonaId = table.Column<int>(type: "int", nullable: false),
                    PeliculaId = table.Column<int>(type: "int", nullable: false),
                    Personaje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_peliculaActor", x => new { x.PeliculaId, x.PersonaId });
                    table.ForeignKey(
                        name: "FK_peliculaActor_peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_peliculaActor_persona_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_generoPeliculas_PeliculaId",
                table: "generoPeliculas",
                column: "PeliculaId");

            migrationBuilder.CreateIndex(
                name: "IX_peliculaActor_PersonaId",
                table: "peliculaActor",
                column: "PersonaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "generoPeliculas");

            migrationBuilder.DropTable(
                name: "peliculaActor");

            migrationBuilder.DropTable(
                name: "genero");

            migrationBuilder.DropTable(
                name: "peliculas");

            migrationBuilder.DropTable(
                name: "persona");
        }
    }
}
