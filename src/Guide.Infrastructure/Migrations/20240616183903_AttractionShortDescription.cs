﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AttractionShortDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "AttractionTranslations",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "AttractionTranslations");
        }
    }
}
