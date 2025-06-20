using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionMaestros.API.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Camiones",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Placa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KilometrajeActual = table.Column<double>(type: "float", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camiones", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Tallers",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CapacidadMaximaDeReparacionesSimultaneas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tallers", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Conductores",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nacionalidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    Genero = table.Column<int>(type: "int", nullable: false),
                    TipoSangre = table.Column<int>(type: "int", nullable: false),
                    CamionCodigo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conductores", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_Conductores_Camiones_CamionCodigo",
                        column: x => x.CamionCodigo,
                        principalTable: "Camiones",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MantenimientosProgramados",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaProgramada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaRealizada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kilometraje = table.Column<double>(type: "float", nullable: false),
                    CamionCodigo = table.Column<int>(type: "int", nullable: false),
                    TallerCodigo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MantenimientosProgramados", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_MantenimientosProgramados_Camiones_CamionCodigo",
                        column: x => x.CamionCodigo,
                        principalTable: "Camiones",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MantenimientosProgramados_Tallers_TallerCodigo",
                        column: x => x.TallerCodigo,
                        principalTable: "Tallers",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Licencias",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoLicencia = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConductorCodigo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licencias", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_Licencias_Conductores_ConductorCodigo",
                        column: x => x.ConductorCodigo,
                        principalTable: "Conductores",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conductores_CamionCodigo",
                table: "Conductores",
                column: "CamionCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_Licencias_ConductorCodigo",
                table: "Licencias",
                column: "ConductorCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_MantenimientosProgramados_CamionCodigo",
                table: "MantenimientosProgramados",
                column: "CamionCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_MantenimientosProgramados_TallerCodigo",
                table: "MantenimientosProgramados",
                column: "TallerCodigo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Licencias");

            migrationBuilder.DropTable(
                name: "MantenimientosProgramados");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Conductores");

            migrationBuilder.DropTable(
                name: "Tallers");

            migrationBuilder.DropTable(
                name: "Camiones");
        }
    }
}
