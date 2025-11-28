using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultorioAPI.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Pagos_PagoId",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_PagoId",
                table: "Turnos");

            migrationBuilder.AlterColumn<int>(
                name: "PagoId",
                table: "Turnos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_PagoId",
                table: "Turnos",
                column: "PagoId",
                unique: true,
                filter: "[PagoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Pagos_PagoId",
                table: "Turnos",
                column: "PagoId",
                principalTable: "Pagos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Pagos_PagoId",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_PagoId",
                table: "Turnos");

            migrationBuilder.AlterColumn<int>(
                name: "PagoId",
                table: "Turnos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_PagoId",
                table: "Turnos",
                column: "PagoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Pagos_PagoId",
                table: "Turnos",
                column: "PagoId",
                principalTable: "Pagos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
