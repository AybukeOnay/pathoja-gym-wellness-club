using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ParentProductId_Add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentProductId",
                table: "Product",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ParentProductId",
                table: "Product",
                column: "ParentProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Product_ParentProductId",
                table: "Product",
                column: "ParentProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Product_ParentProductId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ParentProductId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ParentProductId",
                table: "Product");
        }
    }
}
