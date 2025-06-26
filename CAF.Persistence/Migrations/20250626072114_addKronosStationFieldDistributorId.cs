using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAF.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addKronosStationFieldDistributorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistributorId",
                table: "KRONOS_STATION",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistributorId",
                table: "KRONOS_STATION");
        }
    }
}
