using Microsoft.EntityFrameworkCore.Migrations;

namespace DisneyAPI.Migrations
{
    public partial class disneyDbv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeneroId",
                table: "Peliculas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Generos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Imagen = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Peliculas_GeneroId",
                table: "Peliculas",
                column: "GeneroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Peliculas_Generos_GeneroId",
                table: "Peliculas",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Peliculas_Generos_GeneroId",
                table: "Peliculas");

            migrationBuilder.DropTable(
                name: "Generos");

            migrationBuilder.DropIndex(
                name: "IX_Peliculas_GeneroId",
                table: "Peliculas");

            migrationBuilder.DropColumn(
                name: "GeneroId",
                table: "Peliculas");
        }
    }
}
