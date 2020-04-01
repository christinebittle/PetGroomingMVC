using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using PetGrooming.Models;
using System.ComponentModel.DataAnnotations;

namespace PetGrooming.Data
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    // The application user class is the class that is used to describe someone who is logged in
    // We are leveraging this class by associating it with a Groomer and an Owner.
    
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //A logged in user could be a Groomer
        public virtual Groomer Groomer { get; set; }

        //A logged in user could be an Owner
        public virtual Owner Owner { get; set; }


        //A logged in user could be an Admin
        //note: this could be a separate admin entity
        //a separate admin entity would be suitable if there was
        //more information about an admin we would need to store
        //or relationships to admins that we would need to represent
        //that's not the case here, so this column is fine
        public bool IsAdmin { get; set; }

    }

    //PetGroomingContext class has been adjusted to become a subclass of IdentityDbContext instead of DbContext
    //Why? Because This class helps support base login functionality (IdentityDbContext).
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
