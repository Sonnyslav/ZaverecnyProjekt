using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceCompany.Data.Migrations
{
    /// <inheritdoc />
    public partial class ThirdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsuredEvent_Insurance_InsuranceId",
                table: "InsuredEvent");

            migrationBuilder.AlterColumn<int>(
                name: "InsuranceId",
                table: "InsuredEvent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_InsuredEvent_Insurance_InsuranceId",
                table: "InsuredEvent",
                column: "InsuranceId",
                principalTable: "Insurance",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsuredEvent_Insurance_InsuranceId",
                table: "InsuredEvent");

            migrationBuilder.AlterColumn<int>(
                name: "InsuranceId",
                table: "InsuredEvent",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InsuredEvent_Insurance_InsuranceId",
                table: "InsuredEvent",
                column: "InsuranceId",
                principalTable: "Insurance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
