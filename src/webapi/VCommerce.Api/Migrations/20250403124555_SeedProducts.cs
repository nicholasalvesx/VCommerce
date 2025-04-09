using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VCommerce.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Products(Name, Price, Description, Stock, ImageUrl, CategoryId) " + 
                                 "VALUES('Blusa', 7.66, 'Confortavel', 5, 'blusa.jpeg', 1);");

            migrationBuilder.Sql("INSERT INTO Products(Name, Price, Description, Stock, ImageUrl, CategoryId) " + 
                                 "VALUES('Short', 10.00, 'Confortavel', 12, 'short.jpeg', 1);");

            migrationBuilder.Sql("INSERT INTO Products(Name, Price, Description, Stock, ImageUrl, CategoryId) " + 
                                 "VALUES('Cordao', 7.66, 'Confortavel', 1, 'cordao.jpeg', 2);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Products;");
        }
    }
}