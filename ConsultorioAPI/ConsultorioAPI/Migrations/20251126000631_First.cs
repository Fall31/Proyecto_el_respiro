using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultorioAPI.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleSubjectId",
                table: "Usuarios");

            migrationBuilder.AddColumn<int>(
                name: "PagoId",
                table: "Turnos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PagoId",
                table: "Turnos");

            migrationBuilder.AddColumn<string>(
                name: "GoogleSubjectId",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
