using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebApplication1.Models
{
    public class Context : DbContext
    {
        static Context()
        {
            Database.SetInitializer<Context>(new ContextSeed());
        }

        public Context() : base("Context")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Entity<ServiceParticipation>().HasKey(s => new { s.IncidentId, s.RoleId });
        }

        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentType> IncidentTypes { get; set; }
        public DbSet<ServiceParticipation> ServiceParticipations { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantType> ParticipantTypes { get; set; }
    }
}