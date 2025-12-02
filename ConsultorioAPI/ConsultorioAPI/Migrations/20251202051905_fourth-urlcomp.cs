using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultorioAPI.Migrations
{
    /// <inheritdoc />
    public partial class fourthurlcomp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComprobanteUrl",
                table: "Pagos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComprobanteUrl",
                table: "Pagos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
