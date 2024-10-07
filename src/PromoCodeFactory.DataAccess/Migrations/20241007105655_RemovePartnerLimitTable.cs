using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCodeFactory.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovePartnerLimitTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartnerPromoCodeLimits_Partners_PartnerId",
                table: "PartnerPromoCodeLimits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartnerPromoCodeLimits",
                table: "PartnerPromoCodeLimits");

            migrationBuilder.RenameTable(
                name: "PartnerPromoCodeLimits",
                newName: "PartnerPromoCodeLimit");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerPromoCodeLimits_PartnerId",
                table: "PartnerPromoCodeLimit",
                newName: "IX_PartnerPromoCodeLimit_PartnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartnerPromoCodeLimit",
                table: "PartnerPromoCodeLimit",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerPromoCodeLimit_Partners_PartnerId",
                table: "PartnerPromoCodeLimit",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartnerPromoCodeLimit_Partners_PartnerId",
                table: "PartnerPromoCodeLimit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartnerPromoCodeLimit",
                table: "PartnerPromoCodeLimit");

            migrationBuilder.RenameTable(
                name: "PartnerPromoCodeLimit",
                newName: "PartnerPromoCodeLimits");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerPromoCodeLimit_PartnerId",
                table: "PartnerPromoCodeLimits",
                newName: "IX_PartnerPromoCodeLimits_PartnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartnerPromoCodeLimits",
                table: "PartnerPromoCodeLimits",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerPromoCodeLimits_Partners_PartnerId",
                table: "PartnerPromoCodeLimits",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
