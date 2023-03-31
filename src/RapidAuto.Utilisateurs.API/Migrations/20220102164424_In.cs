using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidAuto.Utilisateurs.API.Migrations
{
    public partial class In : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Addrese",
                table: "Utilisateur",
                newName: "NumeroTel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumeroTel",
                table: "Utilisateur",
                newName: "Addrese");
        }
    }
}
