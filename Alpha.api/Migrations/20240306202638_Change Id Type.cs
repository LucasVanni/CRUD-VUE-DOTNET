using System;
using Alpha.api.Models;
using System.Data;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alpha.api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");


            migrationBuilder.DropColumn(
               name: "Id",
               table: "Products");


            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Products",
                type: "int",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
              name: "PK_Products",
              table: "Products");

            migrationBuilder.DropColumn(
              name: "Id",
              table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false
            );

            migrationBuilder.AddPrimaryKey(
              name: "PK_Products",
              table: "Products",
              column: "Id");
        }
    }
}
