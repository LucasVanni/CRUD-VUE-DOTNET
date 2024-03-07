using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alpha.api.Migrations
{
    /// <inheritdoc />
    public partial class add_extensao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extensao",
                table: "Products",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extensao",
                table: "Products");
        }
    }
}
