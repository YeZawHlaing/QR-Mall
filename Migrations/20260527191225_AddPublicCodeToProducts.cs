using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace product_qr_api.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicCodeToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicCode",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicCode",
                table: "Products");
        }
    }
}
