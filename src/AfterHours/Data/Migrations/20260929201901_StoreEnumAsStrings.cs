using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AfterHours.Data.Migrations
{
    /// <inheritdoc />
    public partial class StoreEnumAsStrings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MediaType",
                table: "MediaItems",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER"
            );

            migrationBuilder.AlterColumn<string>(
                name: "MediaStatus",
                table: "MediaItems",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER"
            );

            migrationBuilder.Sql(
                """
                UPDATE MediaItems
                SET MediaStatus = CASE MediaStatus
                    WHEN '0' THEN 'Completed'
                    WHEN '1' THEN 'Planned'
                    WHEN '2' THEN 'Dropped'
                    ELSE MediaStatus
                END;

                UPDATE MediaItems
                SET MediaType = CASE MediaType
                    WHEN '0' THEN 'Movie'
                    WHEN '1' THEN 'Tv'
                    ELSE MediaType
                END;
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE MediaItems
                SET MediaStatus = CASE MediaStatus
                    WHEN 'Completed' THEN '0'
                    WHEN 'Planned' THEN '1'
                    WHEN 'Dropped' THEN '2'
                    ELSE MediaStatus
                END;

                UPDATE MediaItems
                SET MediaType = CASE MediaType
                    WHEN 'Movie' THEN '0'
                    WHEN 'Tv' THEN '1'
                    ELSE MediaType
                END;
                """
            );

            migrationBuilder.AlterColumn<int>(
                name: "MediaType",
                table: "MediaItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT"
            );

            migrationBuilder.AlterColumn<int>(
                name: "MediaStatus",
                table: "MediaItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT"
            );
        }
    }
}
