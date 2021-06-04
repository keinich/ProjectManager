using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManager.Api.Migrations {
  public partial class CardAddPosition : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.AddColumn<int>(
          name: "PositionX",
          table: "Cards",
          type: "int",
          nullable: false,
          defaultValue: 0);

      migrationBuilder.AddColumn<int>(
          name: "PositionY",
          table: "Cards",
          type: "int",
          nullable: false,
          defaultValue: 0);
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropColumn(
          name: "PositionX",
          table: "Cards");

      migrationBuilder.DropColumn(
          name: "PositionY",
          table: "Cards");
    }
  }
}
