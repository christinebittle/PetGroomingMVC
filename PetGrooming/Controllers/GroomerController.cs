using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using PetGrooming.Models.ViewModels;
using System.Diagnostics;
using System.Globalization; //for cultureinfo.invariantculture
//needed for await
using System.Threading.Tasks;
//needed for other sign in feature classes
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;


namespace PetGrooming.Controllers
{
    public class GroomerController : Controller
    {
        //need this to work with the login functionalities
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        //reference how the Account Controller instantiates the controller class with SignInManager and UserManager

        private PetGroomingContext db = new PetGroomingContext();
        //parameterless constructor function
        public GroomerController(){}
        
        // GET: Groomer
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult List(string groomersearchkey)
        {
            

            Debug.WriteLine("The search key is "+groomersearchkey);

            if (groomersearchkey!=null && groomersearchkey!="")
            {
                //SQL : 
                // select * from groomers 
                // where groomerfname like '%@key%' OR
                // groomerlname like '%key%'
                List<Groomer> Groomers = db.Groomers
                    .Where(groomer => 
                        groomer.GroomerFName.Contains(groomersearchkey) ||
                        groomer.GroomerLName.Contains(groomersearchkey)
                    )
                    .ToList();
                return View(Groomers);
            }
            else
            {
                List<Groomer> Groomers = db.Groomers.ToList();
                return View(Groomers);
            }
        }

        public ActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Add(string Username, string Useremail, string Userpass, string GroomerFName, string GroomerLName, string GroomerDOB, decimal GroomerRate)
        {
            //before creating a groomer, we would like to create a user.
            //this user will be linked with an owner.
            ApplicationUser NewUser = new ApplicationUser();
            NewUser.UserName = Username;
            NewUser.Email = Useremail;
            //code interpreted from AccountController.cs Register Method
            IdentityResult result = await UserManager.CreateAsync(NewUser, Userpass);

            if (result.Succeeded)
            {
                //need to find the user we just created -- get the ID
                string Id = NewUser.Id; //what was the id of the new account?
                //link this id to the Owner
                string GroomerID = Id;



                Groomer NewGroomer = new Groomer();
                NewGroomer.GroomerID = Id;
                NewGroomer.GroomerFName = GroomerFName;
                NewGroomer.GroomerLName = GroomerLName;
                NewGroomer.GroomerDOB = DateTime.ParseExact(GroomerDOB, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                NewGroomer.GroomerRate = (int)(GroomerRate * 100);

                //SQL equivalent : INSERT INTO GROOMERS (GroomerFname, .. ) VALUES (@GroomerFname..)
                db.Groomers.Add(NewGroomer);

                db.SaveChanges();
            }
            else
            {
                //Simple way of displaying errors
                ViewBag.ErrorMessage = "Something Went Wrong!";
                ViewBag.Errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    ViewBag.Errors.Add(error);
                }
            }


            return View();
        }


        public ActionResult Update(string id)
        {
            //select * from groomers where groomerid = @id
            Groomer SelectedGroomer = db.Groomers.Find(id);

            return View(SelectedGroomer);

        }

        [HttpPost]
        public ActionResult Update(string id, string GroomerFName, string GroomerLName, string GroomerDOB, decimal GroomerRate)
        {
         
            //SQL Equivalent : select * from groomers where groomerid = @id
            Groomer SelectedGroomer = db.Groomers.Find(id);
            SelectedGroomer.GroomerFName = GroomerFName;
            SelectedGroomer.GroomerLName = GroomerLName;
            SelectedGroomer.GroomerDOB = DateTime.ParseExact(GroomerDOB, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            SelectedGroomer.GroomerRate = (int)(GroomerRate * 100);

            //SQL equivalent: update groomers set groomerFname = @groomerfname, groomerLname = @groomerlname .. 
            // where groomerid = @id
            db.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult Show(string id)
        {
            //sql equivalent : select * from groomers where groomerid = @id
            Groomer SelectedGroomer = db.Groomers.Find(id);
            //need a list of bookings associated with that groomer
            //sql equivalent : 
            //select * from groombookings where groombookings.groomerid = @id
            //[db.GroomBookings] => Database context groombookings table
            //[.Where()] => maps to the where clause in SQL
            //[booking => booking.GroomerID == id] => where groomerid = @id
            //[.ToList()] => select * from groombookings
            List<GroomBooking> Bookings = db.GroomBookings
                .Include(booking=>booking.GroomServices)
                .Where(booking => booking.GroomerID == id)
                .ToList();

            

            ShowGroomer ViewModel = new ShowGroomer();
            ViewModel.groomer = SelectedGroomer;
            ViewModel.bookings = Bookings;


            return View(ViewModel);
        }

        public ActionResult ConfirmDelete(string id)
        {
            Groomer SelectedGroomer = db.Groomers.Find(id);

            return View(SelectedGroomer);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            //find the groomer
            Groomer SelectedGroomer = db.Groomers.Find(id);
            //sql equivalent : delete from groomers where groomerid=@id 
            db.Groomers.Remove(SelectedGroomer);

            ApplicationUser User = db.Users.Find(id);
            //also remove the account
            db.Users.Remove(User);
            
            db.SaveChanges();

            return RedirectToAction("List");
        }

        //how to get the UserManager and SignInManager from the server
        public GroomerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}