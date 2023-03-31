using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidAuto.Vehicules.API.Migrations
{
    public partial class vehicules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Constructeur = table.Column<string>(type: "TEXT", nullable: false),
                    Modele = table.Column<string>(type: "TEXT", nullable: false),
                    AnneeFabrication = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    NbSieges = table.Column<int>(type: "INTEGER", nullable: false),
                    Couleur = table.Column<string>(type: "TEXT", nullable: false),
                    Niv = table.Column<string>(type: "TEXT", nullable: false),
                    Image1 = table.Column<string>(type: "TEXT", nullable: false),
                    Image2 = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Dispo = table.Column<bool>(type: "INTEGER", nullable: false),
                    Prix = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicule", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicule");
        }
    }
}
