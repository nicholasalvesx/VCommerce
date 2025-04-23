using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VCommerce.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddingClientId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "AspNetUsers");
        }
    }
}
