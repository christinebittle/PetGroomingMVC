using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using PetGrooming.Models;
using PetGrooming.Data;
using System.Diagnostics;

namespace PetGrooming
{
    //I've split the "ApplicationUserManager" class defined in "IdentityConfig.cs"
    //into this file. My plan is to create extra methods that help
    //authenticate the different kinds of users associated with an account
    //NOTE: you also have to modify the "ApplicationUserManager" class so
    //that it is partial as well.
    public partial class ApplicationUserManager : UserManager<ApplicationUser>
    {
        //You could argue having a database reference in this class is a
        //"separation of concerns" issue
        //The other AplicationUserManager class has a static reference to the db
        //static just means we can access the db without "instantiating" the class
        //like we're doing below.
        private PetGroomingContext db = new PetGroomingContext();
        private string userid
        {
            get { return HttpContext.Current.User.Identity.GetUserId() != null ? HttpContext.Current.User.Identity.GetUserId() : ""; }
        }
        private bool isLoggedIn
        {
            get { return HttpContext.Current.User.Identity.IsAuthenticated ? true : false; }
        }
        //public property accessors
        public bool IsLoggedIn
        {
            get { return isLoggedIn; }
        }
        

        //I only want to use this field inside this class, so it is private
        private ApplicationUser GetUser()
        {
            //it will hurt your brain to think about
            //instantiating a class within the class definition itself
            //although, it is possible.
            //I need to do this to easily grab the user.
            //This is considered better practice than using the db to grab the user
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            if (userid == "") return null;
            return(manager.FindById(userid));
        }

        public bool IsUserAdmin()
        {
            ApplicationUser user = GetUser();
            if (user == null) return false;
            if (user.IsAdmin) return true;
            return false;
        }

        public bool IsUserOwner()
        {
            ApplicationUser user = GetUser();
            if (user == null) return false;
            if (user.Owner!=null) return true;
            return false;
        }

        public bool IsUserGroomer()
        {
            ApplicationUser user = GetUser();
            if (user == null) return false;
            if (user.Groomer != null) return true;
            return false;
        }

        //more advanced methods needed such as:
        //is this owner the owner of a pet?
        //is this owner listed as the billed owner for an appointment?
        //is this groomer listed as the groomer for an appointment?

        public bool IsUserPetOwner(Pet pet)
        {
            if (!IsUserOwner()) return false; //not valid because not an owner
            ApplicationUser user = GetUser(); //get user

            //get the pet again
            //"trust, but verify"
            pet = db.Pets.Find(pet.PetID);
            //get the owner and the pets they have
            Owner owner = db.Owners
                .Include(o => o.Pets)
                .Where(o=>o.OwnerID==user.Owner.OwnerID)
                .FirstOrDefault();
            if (owner.Pets.Contains(pet)) return true;

            
            return false; //otherwise invalid
        }

        public bool IsUserBilledOwner(GroomBooking booking)
        {
            if (!IsUserOwner()) return false; //not valid because not an owner
            ApplicationUser user = GetUser(); //get user

            //get the booking again
            //"trust, but verify"
            booking = db.GroomBookings.Find(booking.GroomBookingID);
            //get the owner and the pets they have
            Owner owner = db.Owners
                .Include(o => o.GroomBookings)
                .Where(o => o.OwnerID == user.Owner.OwnerID)
                .FirstOrDefault();
            if (owner.GroomBookings.Contains(booking)) return true;


            return false; //otherwise invalid
        }

        

        public string TestMethod()
        {

            return("Test Successful");
        }
    }
}