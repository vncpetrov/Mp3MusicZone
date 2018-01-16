using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Mp3MusicZone.Data.Migrations
{
    public partial class ChangedSongsDownloadsToListenings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Downloads",
                table: "Songs",
                newName: "Listenings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Listenings",
                table: "Songs",
                newName: "Downloads");
        }
    }
}
