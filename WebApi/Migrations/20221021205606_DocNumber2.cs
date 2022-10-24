using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class DocNumber2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "MovementId",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocNumber",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "MovementId1",
                table: "Payments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MovementId1",
                table: "Payments",
                column: "MovementId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Movements_MovementId1",
                table: "Payments",
                column: "MovementId1",
                principalTable: "Movements",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Movements_MovementId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_MovementId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DocNumber",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "MovementId1",
                table: "Payments");

            migrationBuilder.AlterColumn<long>(
                name: "MovementId",
                table: "Payments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
