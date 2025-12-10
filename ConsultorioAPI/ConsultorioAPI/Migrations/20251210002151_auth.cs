using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultorioAPI.Migrations
{
    /// <inheritdoc />
    public partial class auth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Safely drop FK/index/column only if they exist to avoid errors when DB is out-of-sync
            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Pagos_Turnos_TurnoId' AND parent_object_id = OBJECT_ID(N'dbo.Pagos'))
BEGIN
    ALTER TABLE [dbo].[Pagos] DROP CONSTRAINT [FK_Pagos_Turnos_TurnoId];
END

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Pagos_TurnoId' AND object_id = OBJECT_ID(N'dbo.Pagos'))
BEGIN
    DROP INDEX [IX_Pagos_TurnoId] ON [dbo].[Pagos];
END

IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'TurnoId' AND Object_ID = Object_ID(N'dbo.Pagos'))
BEGIN
    ALTER TABLE [dbo].[Pagos] DROP COLUMN [TurnoId];
END
");

            // Replace the generated AlterColumn with a guarded SQL operation to avoid errors when a default constraint exists
            migrationBuilder.Sql(@"
-- Only alter the column to nullable if it currently exists and is NOT nullable
IF EXISTS (
    SELECT 1 FROM sys.columns c
    WHERE c.object_id = OBJECT_ID(N'dbo.Turnos') AND c.name = N'PagoId' AND c.is_nullable = 0
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'dbo.Turnos') AND [c].[name] = N'PagoId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[Turnos] DROP CONSTRAINT [' + @var0 + '];');

    ALTER TABLE [dbo].[Turnos] ALTER COLUMN [PagoId] int NULL;
END
");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create IX_Turnos_PagoId only if it does not already exist to avoid duplicate index errors
            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes WHERE name = 'IX_Turnos_PagoId' AND object_id = OBJECT_ID(N'dbo.Turnos')
)
BEGIN
    CREATE UNIQUE INDEX [IX_Turnos_PagoId] ON [dbo].[Turnos] ([PagoId]) WHERE [PagoId] IS NOT NULL;
END
");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UsuarioId",
                table: "RefreshTokens",
                column: "UsuarioId");

            // Add FK only if it does not already exist
            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Turnos_Pagos_PagoId' AND parent_object_id = OBJECT_ID(N'dbo.Turnos')
)
BEGIN
    ALTER TABLE [dbo].[Turnos] ADD CONSTRAINT [FK_Turnos_Pagos_PagoId] FOREIGN KEY ([PagoId]) REFERENCES [dbo].[Pagos]([Id]);
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop FK only if it exists
            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Turnos_Pagos_PagoId' AND parent_object_id = OBJECT_ID(N'dbo.Turnos')
)
BEGIN
    ALTER TABLE [dbo].[Turnos] DROP CONSTRAINT [FK_Turnos_Pagos_PagoId];
END
");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            // Drop IX_Turnos_PagoId only if it exists
            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1 FROM sys.indexes WHERE name = 'IX_Turnos_PagoId' AND object_id = OBJECT_ID(N'dbo.Turnos')
)
BEGIN
    DROP INDEX [IX_Turnos_PagoId] ON [dbo].[Turnos];
END
");

            migrationBuilder.AlterColumn<int>(
                name: "PagoId",
                table: "Turnos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
