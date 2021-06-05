﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManager.Api.Migrations {
  public partial class InitialCreate : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
          name: "Cards",
          columns: table => new {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            PositionX = table.Column<int>(type: "int", nullable: false),
            PositionY = table.Column<int>(type: "int", nullable: false),
            Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_Cards", x => x.Id);
          });
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "Cards");
    }
  }
}
