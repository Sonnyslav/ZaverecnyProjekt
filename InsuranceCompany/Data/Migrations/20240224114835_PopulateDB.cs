using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceCompany.Data.Migrations
{
    /// <inheritdoc />
    public partial class PopulateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'7011036219', N'admin@pojistovna.cz', N'ADMIN@POJISTOVNA.CZ', N'admin@pojistovna.cz', N'ADMIN@POJISTOVNA.CZ', 0, N'AQAAAAIAAYagAAAAEPuzGBBGDFfX2AcV9HGnvRB7Jh+DxVm3PoFgTKxAxbBAQLRbXPiI5gkPBzHVxtvaZQ==', N'FFN2THQXYJLOKNIAHBEAGK4QH3JJTW7G', N'3a2e9344-fb77-4966-8b66-da7ac9f1c5d5', NULL, 0, 0, NULL, 1, 0)\r\n");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
