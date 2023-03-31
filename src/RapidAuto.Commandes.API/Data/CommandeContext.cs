using Microsoft.EntityFrameworkCore;
using RapidAuto.Commandes.API.Model;

namespace RapidAuto.Commandes.API.Data
{
    public class CommandeContext : DbContext
    {
        public CommandeContext(DbContextOptions<CommandeContext> options) : base(options)
        {

        }

        public DbSet<EntCommande> Commandes { get; set; }
    }
}
