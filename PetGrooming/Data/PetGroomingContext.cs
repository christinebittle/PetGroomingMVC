using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PetGrooming.Data
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class PetGroomingContext : IdentityDbContext<ApplicationUser>
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public PetGroomingContext() : base("name=PetGroomingContext")
        {
        }
        public static PetGroomingContext Create()
        {
            return new PetGroomingContext();
        }

        public System.Data.Entity.DbSet<PetGrooming.Models.Pet> Pets { get; set; }

        public System.Data.Entity.DbSet<PetGrooming.Models.Species> Species { get; set; }
        public System.Data.Entity.DbSet<PetGrooming.Models.Groomer> Groomers { get; set; }
        public System.Data.Entity.DbSet<PetGrooming.Models.GroomService> GroomServices { get; set; }
        public System.Data.Entity.DbSet<PetGrooming.Models.GroomBooking> GroomBookings { get; set; }
        public System.Data.Entity.DbSet<PetGrooming.Models.Owner> Owners { get; set; }
    }
}
