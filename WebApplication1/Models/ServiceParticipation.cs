using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceParticipation
    {
        /// <summary>
        /// Gets or sets the incident identifier.
        /// </summary>
        /// <value>
        /// The incident identifier.
        /// </value>
        public int IncidentId { get; set; }
        /// <summary>
        /// Gets or sets the incident.
        /// </summary>
        /// <value>
        /// The incident.
        /// </value>
        public virtual Incident Incident { get; set; }

        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>
        public string RoleId { get; set; }
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        public string RoleName { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ServiceParticipation"/> is confirmed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if confirmed; otherwise, <c>false</c>.
        /// </value>
        public bool confirmed { get; set; }

        /// <summary>
        /// Gets or sets the about participation.
        /// </summary>
        /// <value>
        /// The about participation.
        /// </value>
        [Display(Name = "Additional information")]
        [DataType(DataType.MultilineText)]
        public string AboutParticipation { get; set; }
    }
}