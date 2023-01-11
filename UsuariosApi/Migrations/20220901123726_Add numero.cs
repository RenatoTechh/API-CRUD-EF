using Microsoft.EntityFrameworkCore.Migrations;

namespace UsuariosApi.Migrations
{
    public partial class Addnumero : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Numero",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99997,
                column: "ConcurrencyStamp",
                value: "db2bd815-1ea9-4b41-8b34-02d5d86a0857");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99999,
                column: "ConcurrencyStamp",
                value: "4b536683-540e-4946-9951-04c6b43327f8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 99999,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0b1b1f74-85f9-4029-9980-7a76d72a230c", "AQAAAAEAACcQAAAAEGgbsyIHujYDs21e3e+++Ayo7mpsrF6997MjPFOe3AVpXefQu3sMueS+Cr8ukwHuzg==", "a11f7366-bacf-4cc7-9855-89a9d2ac1b70" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Numero",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99997,
                column: "ConcurrencyStamp",
                value: "e3f7cbf5-aae6-4577-be38-d7aa5608b3d9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99999,
                column: "ConcurrencyStamp",
                value: "779d93ab-caeb-40f4-adb6-1f54d70eeaa7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 99999,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d73c25d1-e2ed-45cd-90e1-fd702be902e1", "AQAAAAEAACcQAAAAEIIoPmWDdmPphNpWAmH0go2UKQbs3rcFIrbOXBlT3POBOHEExRnyBZl89c70/M/ckA==", "db7c2961-13e3-4d56-b8b7-5ee12e819713" });
        }
    }
}
