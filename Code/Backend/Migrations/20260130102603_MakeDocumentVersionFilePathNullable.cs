using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class MakeDocumentVersionFilePathNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVersions_Documents_DocumentDocId",
                table: "DocumentVersions");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "DocumentVersions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentDocId",
                table: "DocumentVersions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVersions_Documents_DocumentDocId",
                table: "DocumentVersions",
                column: "DocumentDocId",
                principalTable: "Documents",
                principalColumn: "DocId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVersions_Documents_DocumentDocId",
                table: "DocumentVersions");

            migrationBuilder.UpdateData(
                table: "DocumentVersions",
                keyColumn: "FilePath",
                keyValue: null,
                column: "FilePath",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "DocumentVersions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentDocId",
                table: "DocumentVersions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVersions_Documents_DocumentDocId",
                table: "DocumentVersions",
                column: "DocumentDocId",
                principalTable: "Documents",
                principalColumn: "DocId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
