using Microsoft.EntityFrameworkCore;
using RapidAuto.Vehicules.API.Interfaces;
using RapidAuto.Vehicules.API.Models;

namespace RapidAuto.Vehicules.API.Data
{
    public class VehiculeContext : DbContext
    {
        public VehiculeContext(DbContextOptions<VehiculeContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior =
                 QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Vehicule> Vehicule { get; set; }
    }
}
