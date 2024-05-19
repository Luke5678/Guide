using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AzureStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "AttractionImages",
                newName: "Url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "AttractionImages",
                newName: "Path");
        }
    }
}
