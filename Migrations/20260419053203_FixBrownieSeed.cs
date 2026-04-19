using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BrownieShop.API.Migrations
{
    /// <inheritdoc />
    public partial class FixBrownieSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Brownies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Rich dark chocolate, dense and fudgy. The one that started it all.", "🍫", 60m });

            migrationBuilder.UpdateData(
                table: "Brownies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Classic brownie loaded with crunchy California walnuts on every bite.", "🌰", "Walnut Brownie", 75m });

            migrationBuilder.UpdateData(
                table: "Brownies",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Double chocolate — fudgy base + melted choco chips throughout.", "🍪", "Choco Chip Brownie", 70m });

            migrationBuilder.InsertData(
                table: "Brownies",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 4, "Crushed Oreo cookies baked right into our signature brownie batter.", "⚫", "Oreo Brownie", 80m },
                    { 5, "A swirl of Nutella baked into every bite. Pure indulgence.", "🫙", "Nutella Brownie", 90m },
                    { 6, "Warm brownie topped with vanilla ice cream and chocolate drizzle.", "🍨", "Brownie Sundae", 120m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Brownies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Brownies",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Brownies",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Brownies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "A rich, moist chocolate brownie.", "classic.jpg", 120m });

            migrationBuilder.UpdateData(
                table: "Brownies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Loaded with creamy Nutella.", "nutella.jpg", "Nutella Brownie", 180m });

            migrationBuilder.UpdateData(
                table: "Brownies",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Classic chocolate brownie with crunchy walnuts.", "walnut.jpg", "Walnut Brownie", 150m });
        }
    }
}
