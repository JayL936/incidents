using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.Owin.Security;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Kontroler ról
    /// </summary>
    public class RoleController : Controller
    {
        ApplicationDbContext context;

        public RoleController()
        {
            context = new ApplicationDbContext();
        }

        // GET: Role
        /// <summary>
        /// Wyświetlenie listy ról. Dostępne tylko dla Administratora.
        /// </summary>
        /// <returns>Widok z listą ról.</returns>
        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            var Roles = context.Roles.ToList();
            return View(Roles);
        }

        /// <summary>
        /// Tworzenie nowej roli. Dostępne tylko dla administratora.
        /// </summary>
        /// <returns>Widok z nową rolą.</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        /// <summary>
        /// Zapis nowo utworzonej roli. Dostępne tylko dla administratora.
        /// </summary>
        /// <param name="Role">Nowo tworzona rola.</param>
        /// <returns>Przekierowanie do akcji widoku głównego.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(IdentityRole Role)
        {
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Usuwanie roli. Dostępne tylko dla administratora.
        /// </summary>
        /// <param name="RoleName">Nazwa usuwanej roli pobrana z widoku.</param>
        /// <returns>Przekierowanie do akcji widoku głównego.</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string RoleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Widok zarządzania rolami.
        /// </summary>
        /// <returns>Widok zarządzania rolami.</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult ManageUserRoles()
        {
            PopulateUsersAndRoles();
            return View();
        }

        /// <summary>
        /// Dodawanie użytkownika do wybranej roli.
        /// </summary>
        /// <param name="UserName">Nazwa użytkownika</param>
        /// <param name="RoleName">Nazwa roli</param>
        /// <returns>Widok główny zarządzania rolami.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult AddRoleToUser(string UserName, string RoleName)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (!manager.IsInRole(user.Id, RoleName))
            {
                manager.AddToRole(user.Id, RoleName);
                manager.UpdateSecurityStamp(user.Id);
                ViewBag.ResultMessage = "Role added successfully.";
            }
            else
                ViewBag.ResultMessage = "User already in this role.";

            PopulateUsersAndRoles();

            return View("ManageUserRoles");
        }

        /// <summary>
        /// Wyświetlanie ról użytkownika.
        /// </summary>
        /// <param name="UserName">Nazwa użytkownika.</param>
        /// <returns>Widok główny zarządzania rolami z elementem ViewBag zawierającym role wybranego użytkownika.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                ViewBag.RolesForThisUser = manager.GetRoles(user.Id);
            }
            else
                ViewBag.RolesForThisUser = "User have no roles.";

            PopulateUsersAndRoles();

            return View("ManageUserRoles");
        }

        /// <summary>
        /// Usunięcie użytkownika z roli.
        /// </summary>
        /// <param name="UserName">Nazwa użytkownika.</param>
        /// <param name="RoleName">Nazwa roli.</param>
        /// <returns>Widok główny zarządzania rolami.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUserFromRole(string UserName, string RoleName)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (manager.IsInRole(user.Id, RoleName))
            {
                manager.RemoveFromRole(user.Id, RoleName);
                manager.UpdateSecurityStamp(user.Id);
                ViewBag.ResultMessage = "Role removed for this user.";
            }
            else
                ViewBag.ResultMessage = "This user doesn't belong to selected role.";

            PopulateUsersAndRoles();

            return View("ManageUserRoles");
        }

        /// <summary>
        /// Wypełnienie dwóch list - użytkowników oraz ról.
        /// </summary>
        public void PopulateUsersAndRoles()
        {
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            var users = context.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
                new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = users;
        }
    }
}