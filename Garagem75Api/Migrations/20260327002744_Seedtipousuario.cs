using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garagem75.Api.Migrations
{
    /// <inheritdoc />
    public partial class Seedtipousuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TipoUsuario",
                columns: new[] { "IdTipoUsuario", "DescricaoTipoUsuario" },
                values: new object[,]
                {
                    { 1, "Administrador" },
                    { 2, "Mecânico" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TipoUsuario",
                keyColumn: "IdTipoUsuario",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TipoUsuario",
                keyColumn: "IdTipoUsuario",
                keyValue: 2);
        }
    }
}
