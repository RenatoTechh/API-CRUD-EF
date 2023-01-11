using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiCrud.Migrations
{
    public partial class cep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Cep",
                table: "CentrosDistribuicao",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Cep",
                table: "CentrosDistribuicao",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
