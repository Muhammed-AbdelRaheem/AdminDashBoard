using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderModuleTabels2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BuyerName",
                table: "Orders",
                newName: "BuyerEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BuyerEmail",
                table: "Orders",
                newName: "BuyerName");
        }
    }
}
