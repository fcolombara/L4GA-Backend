using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class RelacionMuchosAMuchosNomina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transportes_Nominas_NominaId",
                table: "Transportes");

            migrationBuilder.DropIndex(
                name: "IX_Transportes_NominaId",
                table: "Transportes");

            migrationBuilder.DropColumn(
                name: "NominaId",
                table: "Transportes");

            migrationBuilder.CreateTable(
                name: "NominaTransportes",
                columns: table => new
                {
                    NominasId = table.Column<int>(type: "int", nullable: false),
                    TransportesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NominaTransportes", x => new { x.NominasId, x.TransportesId });
                    table.ForeignKey(
                        name: "FK_NominaTransportes_Nominas_NominasId",
                        column: x => x.NominasId,
                        principalTable: "Nominas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NominaTransportes_Transportes_TransportesId",
                        column: x => x.TransportesId,
                        principalTable: "Transportes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_NominaTransportes_TransportesId",
                table: "NominaTransportes",
                column: "TransportesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NominaTransportes");

            migrationBuilder.AddColumn<int>(
                name: "NominaId",
                table: "Transportes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transportes_NominaId",
                table: "Transportes",
                column: "NominaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transportes_Nominas_NominaId",
                table: "Transportes",
                column: "NominaId",
                principalTable: "Nominas",
                principalColumn: "Id");
        }
    }
}
