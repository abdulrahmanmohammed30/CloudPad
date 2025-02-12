using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoteTakingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNoteDateTablesNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_AspNetUsers_UserId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Note_AspNetUsers_UserId",
                table: "Note");

            migrationBuilder.DropForeignKey(
                name: "FK_Note_Category_CategoryId",
                table: "Note");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteTags_Note_NotesNoteId",
                table: "NoteTags");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteTags_Tag_TagsTagId",
                table: "NoteTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Resource_Note_NoteId",
                table: "Resource");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_AspNetUsers_UserId",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Resource",
                table: "Resource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Note",
                table: "Note");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "Resource",
                newName: "Resources");

            migrationBuilder.RenameTable(
                name: "Note",
                newName: "Notes");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_Tag_UserId",
                table: "Tags",
                newName: "IX_Tags_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tag_Name",
                table: "Tags",
                newName: "IX_Tags_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Resource_NoteId",
                table: "Resources",
                newName: "IX_Resources_NoteId");

            migrationBuilder.RenameIndex(
                name: "IX_Note_UserId",
                table: "Notes",
                newName: "IX_Notes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Note_NoteGuid",
                table: "Notes",
                newName: "IX_Notes_NoteGuid");

            migrationBuilder.RenameIndex(
                name: "IX_Note_CategoryId",
                table: "Notes",
                newName: "IX_Notes_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_UserId",
                table: "Categories",
                newName: "IX_Categories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_Name",
                table: "Categories",
                newName: "IX_Categories_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Category_CategoryGuid",
                table: "Categories",
                newName: "IX_Categories_CategoryGuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Resources",
                table: "Resources",
                column: "ResourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notes",
                table: "Notes",
                column: "NoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_UserId",
                table: "Notes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Categories_CategoryId",
                table: "Notes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTags_Notes_NotesNoteId",
                table: "NoteTags",
                column: "NotesNoteId",
                principalTable: "Notes",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTags_Tags_TagsTagId",
                table: "NoteTags",
                column: "TagsTagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Notes_NoteId",
                table: "Resources",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetUsers_UserId",
                table: "Tags",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_UserId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_UserId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Categories_CategoryId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteTags_Notes_NotesNoteId",
                table: "NoteTags");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteTags_Tags_TagsTagId",
                table: "NoteTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Notes_NoteId",
                table: "Resources");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetUsers_UserId",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Resources",
                table: "Resources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notes",
                table: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.RenameTable(
                name: "Resources",
                newName: "Resource");

            migrationBuilder.RenameTable(
                name: "Notes",
                newName: "Note");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_UserId",
                table: "Tag",
                newName: "IX_Tag_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_Name",
                table: "Tag",
                newName: "IX_Tag_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Resources_NoteId",
                table: "Resource",
                newName: "IX_Resource_NoteId");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_UserId",
                table: "Note",
                newName: "IX_Note_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_NoteGuid",
                table: "Note",
                newName: "IX_Note_NoteGuid");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_CategoryId",
                table: "Note",
                newName: "IX_Note_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_UserId",
                table: "Category",
                newName: "IX_Category_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_Name",
                table: "Category",
                newName: "IX_Category_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_CategoryGuid",
                table: "Category",
                newName: "IX_Category_CategoryGuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Resource",
                table: "Resource",
                column: "ResourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Note",
                table: "Note",
                column: "NoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_AspNetUsers_UserId",
                table: "Category",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Note_AspNetUsers_UserId",
                table: "Note",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Category_CategoryId",
                table: "Note",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTags_Note_NotesNoteId",
                table: "NoteTags",
                column: "NotesNoteId",
                principalTable: "Note",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTags_Tag_TagsTagId",
                table: "NoteTags",
                column: "TagsTagId",
                principalTable: "Tag",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_Note_NoteId",
                table: "Resource",
                column: "NoteId",
                principalTable: "Note",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_AspNetUsers_UserId",
                table: "Tag",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
