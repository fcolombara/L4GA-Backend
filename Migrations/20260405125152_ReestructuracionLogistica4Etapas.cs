using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class ReestructuracionLogistica4Etapas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Fecha = table.Column<DateTime>(type: "date", nullable: false),
                    TransporteId = table.Column<int>(type: "int", nullable: false),
                    HoraArriboCremer = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    HoraCargaNeutroCremer = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    HoraOutCremer = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    TaraCremer = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    PesoCargadoCremer = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    PesoTotalCremer = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    LitrosCremer = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    HoraArriboGreen = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    HoraInPlantaGreen = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    HoraDescargaGreen = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    VolDescargadoGreen = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    PesoGreenIngreso = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    HoraEquipoListoGreen = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    HoraCargaBioGreen = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    HoraOutGreen = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    VolCargadoGreen = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    PesoGreenEgreso = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    HoraArriboPuerto = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    HoraInUnidadPuerto = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    PesajePuerto = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    PesoRecibidoPuerto = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    LitrosRecibidosPuerto = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    TrackingLink = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operaciones_Transportes_TransporteId",
                        column: x => x.TransporteId,
                        principalTable: "Transportes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Operaciones_TransporteId",
                table: "Operaciones",
                column: "TransporteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operaciones");
        }
    }
}
