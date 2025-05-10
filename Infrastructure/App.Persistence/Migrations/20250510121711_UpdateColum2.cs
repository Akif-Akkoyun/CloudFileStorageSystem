using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColum2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileId1",
                table: "FileShares",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FileShares_FileId1",
                table: "FileShares",
                column: "FileId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FileShares_Files_FileId1",
                table: "FileShares",
                column: "FileId1",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileShares_Files_FileId1",
                table: "FileShares");

            migrationBuilder.DropIndex(
                name: "IX_FileShares_FileId1",
                table: "FileShares");

            migrationBuilder.DropColumn(
                name: "FileId1",
                table: "FileShares");
        }
    }
}
