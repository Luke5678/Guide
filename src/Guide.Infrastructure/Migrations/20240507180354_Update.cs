using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "AttractionTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AttractionTranslations");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "AttractionTranslations");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AttractionTranslations");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "AttractionTranslations");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "AttractionTranslations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "AttractionTranslations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AttractionTranslations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "AttractionTranslations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AttractionTranslations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "AttractionTranslations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "AttractionTranslations",
                type: "TEXT",
                nullable: true);
        }
    }
}
