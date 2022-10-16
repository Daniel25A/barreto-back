using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class OneToManyClientsObligation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obligations_Clients_ClientId",
                table: "Obligations");

            migrationBuilder.DropIndex(
                name: "IX_Obligations_ClientId",
                table: "Obligations");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Obligations");

            migrationBuilder.CreateTable(
                name: "ClientObligation",
                columns: table => new
                {
                    ClientsId = table.Column<long>(type: "bigint", nullable: false),
                    ObligationsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientObligation", x => new { x.ClientsId, x.ObligationsId });
                    table.ForeignKey(
                        name: "FK_ClientObligation_Clients_ClientsId",
                        column: x => x.ClientsId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientObligation_Obligations_ObligationsId",
                        column: x => x.ObligationsId,
                        principalTable: "Obligations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientObligation_ObligationsId",
                table: "ClientObligation",
                column: "ObligationsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientObligation");

            migrationBuilder.AddColumn<long>(
                name: "ClientId",
                table: "Obligations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Obligations_ClientId",
                table: "Obligations",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Obligations_Clients_ClientId",
                table: "Obligations",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
