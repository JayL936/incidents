using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace WebApplication1.Models
{
    public class ContextSeed : System.Data.Entity.DropCreateDatabaseIfModelChanges<Context>
    {
        ApplicationDbContext appContext = new ApplicationDbContext();

        protected override void Seed(Context context)
        {

            base.Seed(context);

            var incidents = new List<Incident>
            {
                new Incident{Type="Wypadek", Lat=52.229491, Long=21.002137, About="Incydent testowy", AddDate=DateTime.Now, 
                    DateOfIncident=DateTime.Parse("2010-09-30"), TimeOfIncident=TimeSpan.Parse("15:30:00"), City="Warsaw", Address="Twarda 30", TypeID=1}
            };

            // incidents.ForEach(i => context.Incidents.Add(i));
            context.SaveChanges();

            SeedRoles();
        }

        private void SeedRoles()
        {
            var roles = new List<IdentityRole>
            {
                new IdentityRole{Name = "Admin"},
                new IdentityRole{Name = "Police"},
                new IdentityRole{Name = "Emergency"},
                new IdentityRole{Name = "Municipal police"},
                new IdentityRole{Name = "Fire department"},
                new IdentityRole{Name = "City cleaning"},
                new IdentityRole{Name = "Other"}
            };

            roles.ForEach(r => appContext.Roles.Add(r));
            appContext.SaveChanges();

            if (!appContext.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(appContext);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!appContext.Users.Any(u => u.UserName == "abc@abc.pl"))
            {
                var store = new UserStore<ApplicationUser>(appContext);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "abc@abc.pl", Email = "abc@abc.pl" };

                manager.Create(user, "123456");
                manager.AddToRole(user.Id, "Admin");
            }
        }
    }
}