using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CowManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Nickname2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NickName",
                table: "AspNetUsers",
                newName: "Nickname");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nickname",
                table: "AspNetUsers",
                newName: "NickName");
        }
    }
}
