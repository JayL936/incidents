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

        public DbSet<Incident> Incidents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<ServiceParticipation>()
            //    .HasRequired(s => s.Incident)
            //    .WithOptional()
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<ServiceParticipation>()
            //    .HasRequired(s => s.IdentityRole)
            //    .WithOptional()
            //    .WillCascadeOnDelete(true);

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Entity<ServiceParticipation>().HasKey(s => new { s.IncidentId, s.RoleId });
        }

        public DbSet<IncidentType> IncidentTypes { get; set; }
        public DbSet<ServiceParticipation> ServiceParticipations { get; set; }
    }
}