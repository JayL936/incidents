using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebApplication1.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public class Context : DbContext
    {
        /// <summary>
        /// Initializes the <see cref="Context"/> class.
        /// </summary>
        static Context()
        {
            Database.SetInitializer<Context>(new ContextSeed());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        public Context() : base("Context")
        {

        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Entity<ServiceParticipation>().HasKey(s => new { s.IncidentId, s.RoleId });
        }

        /// <summary>
        /// Gets or sets the incidents.
        /// </summary>
        /// <value>
        /// The incidents.
        /// </value>
        public DbSet<Incident> Incidents { get; set; }
        /// <summary>
        /// Gets or sets the incident types.
        /// </summary>
        /// <value>
        /// The incident types.
        /// </value>
        public DbSet<IncidentType> IncidentTypes { get; set; }
        /// <summary>
        /// Gets or sets the service participations.
        /// </summary>
        /// <value>
        /// The service participations.
        /// </value>
        public DbSet<ServiceParticipation> ServiceParticipations { get; set; }
        /// <summary>
        /// Gets or sets the participants.
        /// </summary>
        /// <value>
        /// The participants.
        /// </value>
        public DbSet<Participant> Participants { get; set; }
        /// <summary>
        /// Gets or sets the participant types.
        /// </summary>
        /// <value>
        /// The participant types.
        /// </value>
        public DbSet<ParticipantType> ParticipantTypes { get; set; }
    }
}