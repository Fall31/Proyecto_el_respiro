using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultorioAPI.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.Sql(@"IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Pagos_Turnos_TurnoId' AND parent_object_id = OBJECT_ID(N'Pagos'))
BEGIN
    ALTER TABLE [Pagos] DROP CONSTRAINT [FK_Pagos_Turnos_TurnoId];
END
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Pagos_TurnoId' AND object_id = OBJECT_ID(N'Pagos'))
BEGIN
    DROP INDEX [IX_Pagos_TurnoId] ON [Pagos];
END
IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'TurnoId' AND Object_ID = Object_ID(N'Pagos'))
BEGIN
    ALTER TABLE [Pagos] DROP COLUMN [TurnoId];
END");
            }
            else
            {
                migrationBuilder.DropForeignKey(
                    name: "FK_Pagos_Turnos_TurnoId",
                    table: "Pagos");

                migrationBuilder.DropIndex(
                    name: "IX_Pagos_TurnoId",
                    table: "Pagos");

                migrationBuilder.DropColumn(
                    name: "TurnoId",
                    table: "Pagos");
            }

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Pagos_PagoId",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_PagoId",
                table: "Turnos");

            migrationBuilder.AddColumn<int>(
                name: "TurnoId",
                table: "Pagos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_TurnoId",
                table: "Pagos",
                column: "TurnoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Turnos_TurnoId",
                table: "Pagos",
                column: "TurnoId",
                principalTable: "Turnos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
