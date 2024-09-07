using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagement.Migrations
{
    /// <inheritdoc />
    public partial class updaterelationinRoomtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_HotelBranches_HotelBranchBranchId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_HotelBranchBranchId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "HotelBranchBranchId",
                table: "Rooms");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BranchId",
                table: "Rooms",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_HotelBranches_BranchId",
                table: "Rooms",
                column: "BranchId",
                principalTable: "HotelBranches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_HotelBranches_BranchId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_BranchId",
                table: "Rooms");

            migrationBuilder.AddColumn<int>(
                name: "HotelBranchBranchId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HotelBranchBranchId",
                table: "Rooms",
                column: "HotelBranchBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_HotelBranches_HotelBranchBranchId",
                table: "Rooms",
                column: "HotelBranchBranchId",
                principalTable: "HotelBranches",
                principalColumn: "BranchId");
        }
    }
}
