using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smartRestaurant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsedAtToUserToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationToken",
                table: "UserTokens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailConfirmationTokenExpiresAt",
                table: "UserTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UsedAt",
                table: "UserTokens",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmationToken",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "EmailConfirmationTokenExpiresAt",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "UsedAt",
                table: "UserTokens");
        }
    }
}
