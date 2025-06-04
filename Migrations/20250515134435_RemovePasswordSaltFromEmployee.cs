using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternationalBankAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemovePasswordSaltFromEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SentAt",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SentToSWIFT",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SentToSWIFT",
                table: "Payments");
        }
    }
}
