using Microsoft.EntityFrameworkCore;
using RapidAuto.Utilisateurs.API.Models;

namespace RapidAuto.Utilisateurs.API.Data
{
    public class UtilisateurContext : DbContext
    {
        public UtilisateurContext(DbContextOptions<UtilisateurContext> options) : base(options)
        {

        }

        public DbSet<Utilisateur> Utilisateur { get; set; }
    }
}
