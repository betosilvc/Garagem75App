using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garagem75.Api.Migrations
{
    /// <inheritdoc />
    public partial class OSatualizada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "OrdemServicos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "OrdemServicos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrdemServicos_ClienteId",
                table: "OrdemServicos",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdemServicos_Cliente_ClienteId",
                table: "OrdemServicos",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "IdCliente",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdemServicos_Cliente_ClienteId",
                table: "OrdemServicos");

            migrationBuilder.DropIndex(
                name: "IX_OrdemServicos_ClienteId",
                table: "OrdemServicos");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "OrdemServicos");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "OrdemServicos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
