using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManager.Api.Migrations {
  public partial class CardsAddUserId : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.AddColumn<string>(
          name: "UserId",
          table: "Cards",
          type: "nvarchar(max)",
          nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropColumn(
          name: "UserId",
          table: "Cards");
    }
  }
}
