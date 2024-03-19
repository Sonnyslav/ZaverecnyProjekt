using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceCompany.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateofEvents : Migration
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
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "InsuredEvent",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InsuredEvent_ClientId",
                table: "InsuredEvent",
                column: "ClientId");



            migrationBuilder.AddForeignKey(
                name: "FK_InsuredEvent_Insurance_InsuranceId",
                table: "InsuredEvent",
                column: "InsuranceId",
                principalTable: "Insurance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsuredEvent_Client_ClientId",
                table: "InsuredEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_InsuredEvent_Insurance_InsuranceId",
                table: "InsuredEvent");

            migrationBuilder.DropIndex(
                name: "IX_InsuredEvent_ClientId",
                table: "InsuredEvent");

            migrationBuilder.DropColumn(
                name: "ClientId",
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
    }
}
