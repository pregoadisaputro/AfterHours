using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AfterHours.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBackdropAndReleaseDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackdropPath",
                table: "MediaItems");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "MediaItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackdropPath",
                table: "MediaItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ReleaseDate",
                table: "MediaItems",
                type: "TEXT",
                nullable: true);
        }
    }
}
