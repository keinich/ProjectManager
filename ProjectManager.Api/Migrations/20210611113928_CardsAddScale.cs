using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManager.Api.Migrations {
  public partial class CardsAddScale : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.AddColumn<int>(
          name: "Height",
          table: "Cards",
          type: "int",
          nullable: false,
          defaultValue: 0);

      migrationBuilder.AddColumn<int>(
          name: "Width",
          table: "Cards",
          type: "int",
          nullable: false,
          defaultValue: 0);
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropColumn(
          name: "Height",
          table: "Cards");

      migrationBuilder.DropColumn(
          name: "Width",
          table: "Cards");
    }
  }
}
