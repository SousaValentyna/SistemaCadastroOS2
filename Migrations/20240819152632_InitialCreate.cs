using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaCadastroOS2.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OSModels",
                columns: table => new
                {
                    NumeroOS = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TituloServico = table.Column<string>(type: "text", nullable: false),
                    CNPJ = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: false),
                    NomeDoCliente = table.Column<string>(type: "text", nullable: false),
                    CPF = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    NomeDoPrestador = table.Column<string>(type: "text", nullable: false),
                    DataExecucaoServico = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValorDoServico = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OSModels", x => x.NumeroOS);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OSModels");
        }
    }
}
