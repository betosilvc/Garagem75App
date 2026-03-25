using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garagem75.Api.Migrations
{
    /// <inheritdoc />
    public partial class fotourl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "Veiculo",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "Veiculo");
        }
    }
}
