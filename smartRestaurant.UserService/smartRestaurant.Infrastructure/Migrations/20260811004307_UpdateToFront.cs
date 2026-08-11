using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smartRestaurant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToFront : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "ActiveCompanyId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveCompanyId",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
