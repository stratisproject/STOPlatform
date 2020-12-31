using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Stratis.STOPlatform.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(nullable: true),
                    Json = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: false),
                    WalletAddress = table.Column<string>(nullable: true),
                    LastCheck = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deposits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    TransactionId = table.Column<string>(nullable: false),
                    Invested = table.Column<decimal>(nullable: false),
                    EarnedToken = table.Column<decimal>(nullable: false),
                    Refunded = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deposits_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "Json", "Key" },
                values: new object[] { 1, "{\"name\":\"Your STO\",\"symbol\":\"TOKEN\",\"totalSupply\":0,\"showTotalContribution\":false,\"pageCover\":\"Lorem ipsum dolor sit amet, pro an agam audire euismod, pro et tritani persequeris. Graece accumsan et eos. Harum doming inermis ut vis, sea eu adipiscing complectitur.\\r\\n\\r\\nEos ad legimus inimicus, dico purto cu qui, et percipit torquatos interpretaris mea. Ex solum consequat percipitur vim, quas melius delicatissimi mel ei.\",\"websiteUrl\":\"https://mystowebsite.com\",\"blogPostUrl\":\"https://stratisplatform.com/blog/\",\"termsAndConditionsUrl\":\"https://stratisplatform.com/terms-of-use\",\"logoSrc\":\"/images/default-logo.png\",\"backgroundSrc\":\"/images/default-bg.png\",\"loginBackgroundSrc\":\"/images/default-bg.png\"}", "STOSetting" });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "Json", "Key" },
                values: new object[] { 2, "{\"done\":false,\"currentStep\":1}", "SetupSetting" });

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_UserId",
                table: "Deposits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_TransactionId_UserId",
                table: "Deposits",
                columns: new[] { "TransactionId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Key",
                table: "Documents",
                column: "Key",
                unique: true,
                filter: "[Key] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Address",
                table: "Users",
                column: "Address",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deposits");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
