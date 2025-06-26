using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CAF.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addKronosStation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KRONOS_STATION",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StationCode = table.Column<int>(type: "integer", nullable: false),
                    StationName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SetupDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Platform = table.Column<int>(type: "integer", nullable: false),
                    ServiceTypes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Source = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastSynced = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KRONOS_STATION", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KRONOS_STATION_StationCode",
                table: "KRONOS_STATION",
                column: "StationCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KRONOS_STATION");
        }
    }
}
