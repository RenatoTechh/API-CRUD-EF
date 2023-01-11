using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace ApiCrud.Migrations
{
    public partial class CD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CentroDeDistribuicao",
                table: "Produtos");

            migrationBuilder.AddColumn<int>(
                name: "CentroDistribuicaoId",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CentrosDistribuicao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Cep = table.Column<int>(type: "int", nullable: false),
                    Logradouro = table.Column<string>(type: "text", nullable: true),
                    Complemento = table.Column<string>(type: "text", nullable: true),
                    Bairro = table.Column<string>(type: "text", nullable: true),
                    Localidade = table.Column<string>(type: "text", nullable: true),
                    Uf = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentrosDistribuicao", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CentroDistribuicaoId",
                table: "Produtos",
                column: "CentroDistribuicaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_CentrosDistribuicao_CentroDistribuicaoId",
                table: "Produtos",
                column: "CentroDistribuicaoId",
                principalTable: "CentrosDistribuicao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_CentrosDistribuicao_CentroDistribuicaoId",
                table: "Produtos");

            migrationBuilder.DropTable(
                name: "CentrosDistribuicao");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_CentroDistribuicaoId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "CentroDistribuicaoId",
                table: "Produtos");

            migrationBuilder.AddColumn<double>(
                name: "CentroDeDistribuicao",
                table: "Produtos",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
