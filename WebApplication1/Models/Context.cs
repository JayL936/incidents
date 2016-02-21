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
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<WebApplication1.Models.IncidentType> IncidentTypes { get; set; }
    }
}