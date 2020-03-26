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
//needed for await
using System.Threading.Tasks;
//needed for other sign in feature classes
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace PetGrooming.Controllers
{
    public class OwnerController : Controller
    {
        //need this to work with the login functionalities
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        //reference how the Account Controller instantiates the controller class with SignInManager and UserManager


        private PetGroomingContext db = new PetGroomingContext();
        //paramaterless constructor
        public OwnerController(){}

        // GET: GroomService/List
        public ActionResult List()
        {
            //How could we modify this to include a search bar?
            List<Owner> owners = db.Owners.SqlQuery("Select * from Owners").ToList();
            return View(owners);

        }


        public ActionResult New()
        {

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Add(string Username, string Useremail, string Userpass, string OwnerFname, string OwnerLname, string OwnerAddress, string OwnerWorkPhone, string OwnerHomePhone)
        {
            //before creating an owner, we would like to create a user.
            //this user will be linked with an owner.
            ApplicationUser NewUser = new ApplicationUser();
            NewUser.UserName = Username;
            NewUser.Email = Useremail;
            //code interpreted from AccountController.cs Register Method
            IdentityResult result = await UserManager.CreateAsync(NewUser, Userpass);

            //only create owner if account is created
            if (result.Succeeded) {
                //need to find the user we just created -- get the ID
                string Id = NewUser.Id; //what was the id of the new account?
                //link this id to the Owner
                string OwnerID = Id;

                
                string query = "insert into Owners (OwnerFname, OwnerLname, OwnerAddress, OwnerWorkPhone, OwnerHomePhone,OwnerID) values (@OwnerFname, @OwnerLname, @OwnerAddress, @OwnerWorkPhone, @OwnerHomePhone,@id)";

                SqlParameter[] sqlparams = new SqlParameter[6];
                sqlparams[0] = new SqlParameter("@OwnerFname", OwnerFname);
                sqlparams[1] = new SqlParameter("@OwnerLname", OwnerLname);
                sqlparams[2] = new SqlParameter("@OwnerAddress", OwnerAddress);
                sqlparams[3] = new SqlParameter("@OwnerWorkPhone", OwnerWorkPhone);
                sqlparams[4] = new SqlParameter("@OwnerHomePhone", OwnerHomePhone);
                sqlparams[5] = new SqlParameter("@id", OwnerID);


                db.Database.ExecuteSqlCommand(query, sqlparams);
            }
            else
            {
                //Simple way of displaying errors
                ViewBag.ErrorMessage = "Something Went Wrong!";
                ViewBag.Errors = new List<string>();
                foreach(var error in result.Errors)
                {
                    ViewBag.Errors.Add(error);
                }
            }

            return View("New");
        }



        public ActionResult Show(string id)
        {
            //find data about the individual owner
            string main_query = "select * from Owners where OwnerID = @id";
            var pk_parameter = new SqlParameter("@id", id);
            Owner Owner = db.Owners.SqlQuery(main_query, pk_parameter).FirstOrDefault();

            //find data about all pets that owner has (through id)
            //remember to check the generated column names! (SQL Server Object Explorer)
            string aside_query = "select * from Pets inner join PetOwners on Pets.PetID = PetOwners.Pet_PetID where PetOwners.Owner_OwnerID=@id";
            var fk_parameter = new SqlParameter("@id",id);
            List<Pet> OwnedPets = db.Pets.SqlQuery(aside_query, fk_parameter).ToList();

            //find data about GroomBookings billed to this owner
            string booking_query = "select * from GroomBookings where GroomBookings.OwnerID=@id";
            var booking_parameter = new SqlParameter("@id",id);
            List<GroomBooking> BilledGrooms = db.GroomBookings.SqlQuery(booking_query, booking_parameter).ToList();


            string all_pets_query = "select * from Pets";
            List<Pet> AllPets = db.Pets.SqlQuery(all_pets_query).ToList();

            //ViewModel is a hybrid of three classifications of information
            //(1) showing the classic owner data
            //(2) showing all pets that owner has
            //(3) showing all pets in general (for ADD)
            ShowOwner viewmodel = new ShowOwner();
            viewmodel.owner = Owner;
            viewmodel.pets = OwnedPets;
            viewmodel.all_pets = AllPets;
            viewmodel.billedgrooms = BilledGrooms;

            return View(viewmodel);
        }


        // this method inserts into the bridging table
        // it is assumed we know the owner (view is access from show owner)
        // just need the pet id
        [HttpPost]
        public ActionResult AttachPet(string id, int PetID)
        {
            Debug.WriteLine("owner id is"+id+" and petid is "+PetID);

            //first, check if that pet is already owned by that owner
            string check_query = "select * from Pets inner join PetOwners on PetOwners.Pet_PetID = Pets.PetID where Pet_PetID=@PetID and Owner_OwnerID=@id";
            SqlParameter[] check_params = new SqlParameter[2];
            check_params[0] = new SqlParameter("@id", id);
            check_params[1] = new SqlParameter("@PetID", PetID);
            List<Pet> pets = db.Pets.SqlQuery(check_query, check_params).ToList();
            //only execute add if the pet isn't already owned by that owner!
            if (pets.Count <= 0) { 


                //first id above is the ownerid, then the petid
                string query = "insert into PetOwners (Pet_PetID, Owner_OwnerID) values (@PetID, @id)";
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@id", id);
                sqlparams[1] = new SqlParameter("@PetID",PetID);


                db.Database.ExecuteSqlCommand(query, sqlparams);
            }

            return RedirectToAction("Show/"+id);

        }


        //URL: /Owner/DetachPet/id?PetID=pid
        [HttpGet]
        public ActionResult DetachPet(string id, int PetID)
        {
            //This method is a more rare instance where two items are passed through a GET URL
            Debug.WriteLine("owner id is" + id + " and petid is " + PetID);

            string query = "delete from PetOwners where Pet_PetID=@PetID and Owner_OwnerID=@id";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@PetID",PetID);
            sqlparams[1] = new SqlParameter("@id",id);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("Show/"+id);
        }

        //
        public ActionResult Update(string id)
        {
            string query = "select * from Owners where OwnerID = @id";
            var parameter = new SqlParameter("@id", id);
            Owner owner = db.Owners.SqlQuery(query, parameter).FirstOrDefault();

            return View(owner);
        }


        [HttpPost]
        public ActionResult Update(string id, string OwnerFname, string OwnerLname, string OwnerAddress, string OwnerWorkPhone, string OwnerHomePhone)
        {
            string query = "update Owners set OwnerFname=@OwnerFname, OwnerLname=@OwnerLname, OwnerAddress=@OwnerAddress, OwnerWorkPhone=@OwnerWorkPhone, OwnerHomePhone=@OwnerHomePhone where OwnerID = @id";

            SqlParameter[] sqlparams = new SqlParameter[6];
            
            sqlparams[0] = new SqlParameter("@OwnerFname", OwnerFname);
            sqlparams[1] = new SqlParameter("@OwnerLname", OwnerLname);
            sqlparams[2] = new SqlParameter("@OwnerAddress", OwnerAddress);
            sqlparams[3] = new SqlParameter("@OwnerWorkPhone", OwnerWorkPhone);
            sqlparams[4] = new SqlParameter("@OwnerHomePhone", OwnerHomePhone);
            sqlparams[5] = new SqlParameter("@id", id);


            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }

        public ActionResult DeleteConfirm(string id)
        {
            string query = "select * from Owners where OwnerID=@id";
            SqlParameter param = new SqlParameter("@id", id);
            Owner owner = db.Owners.SqlQuery(query, param).FirstOrDefault();
            return View(owner);
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            //delete from associations to pet
            string pet_owners_query = "delete from PetOwners where Owner_OwnerID=@id";
            db.Database.ExecuteSqlCommand(pet_owners_query, new SqlParameter("@id",id));


            string query = "delete from Owners where OwnerID=@id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);

            //delete associated account
            //(Account has same id -- one to one via fk and primary key).
            string account_query = "delete from AspNetUsers where Id=@id";
            db.Database.ExecuteSqlCommand(account_query, new SqlParameter("@id",id));

            return RedirectToAction("List");
        }

        //how to get the UserManager and SignInManager from the server
        public OwnerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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