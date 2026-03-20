using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddNominaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NominaId",
                table: "Transportes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Nominas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaCarga = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaActividad = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nominas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transportes_Nominas_NominaId",
                table: "Transportes");

            migrationBuilder.DropTable(
                name: "Nominas");

            migrationBuilder.DropIndex(
                name: "IX_Transportes_NominaId",
                table: "Transportes");

            migrationBuilder.DropColumn(
                name: "NominaId",
                table: "Transportes");
        }
    }
}
