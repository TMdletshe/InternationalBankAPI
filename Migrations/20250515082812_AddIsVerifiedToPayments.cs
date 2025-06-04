using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternationalBankAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIsVerifiedToPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "Payments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "Payments");
        }
    }
}
