using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garagem75.Api.Migrations
{
    /// <inheritdoc />
    public partial class uniques : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Veiculo_Placa",
                table: "Veiculo",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Peca_CodPeca",
                table: "Peca",
                column: "CodPeca",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Cpf",
                table: "Cliente",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Email",
                table: "Cliente",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Veiculo_Placa",
                table: "Veiculo");

            migrationBuilder.DropIndex(
                name: "IX_Peca_CodPeca",
                table: "Peca");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_Cpf",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_Email",
                table: "Cliente");
        }
    }
}
