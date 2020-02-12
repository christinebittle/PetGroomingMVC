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

namespace PetGrooming.Controllers
{
    public class OwnerController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();

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
        public ActionResult Add(string OwnerFname, string OwnerLname, string OwnerAddress, string OwnerWorkPhone, string OwnerHomePhone)
        {
            string query = "insert into Owners (OwnerFname, OwnerLname, OwnerAddress, OwnerWorkPhone, OwnerHomePhone) values (@OwnerFname, @OwnerLname, @OwnerAddress, @OwnerWorkPhone, @OwnerHomePhone)";

            SqlParameter[] sqlparams = new SqlParameter[5];
            sqlparams[0] = new SqlParameter("@OwnerFname", OwnerFname);
            sqlparams[1] = new SqlParameter("@OwnerLname", OwnerLname);
            sqlparams[2] = new SqlParameter("@OwnerAddress", OwnerAddress);
            sqlparams[3] = new SqlParameter("@OwnerWorkPhone", OwnerWorkPhone);
            sqlparams[4] = new SqlParameter("@OwnerHomePhone", OwnerHomePhone);


            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }



        public ActionResult Show(int id)
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

            return View(viewmodel);
        }


        // this method inserts into the bridging table
        // it is assumed we know the owner (view is access from show owner)
        // just need the pet id
        [HttpPost]
        public ActionResult AttachPet(int id, int PetID)
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
        public ActionResult DetachPet(int id, int PetID)
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
        public ActionResult Update(int id)
        {
            string query = "select * from Owners where OwnerID = @id";
            var parameter = new SqlParameter("@id", id);
            Owner owner = db.Owners.SqlQuery(query, parameter).FirstOrDefault();

            return View(owner);
        }


        [HttpPost]
        public ActionResult Update(int id, string OwnerFname, string OwnerLname, string OwnerAddress, string OwnerWorkPhone, string OwnerHomePhone)
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

        public ActionResult DeleteConfirm(int id)
        {
            string query = "select * from Owners where OwnerID=@id";
            SqlParameter param = new SqlParameter("@id", id);
            Owner owner = db.Owners.SqlQuery(query, param).FirstOrDefault();
            return View(owner);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string query = "delete from Owners where OwnerID=@id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);



            return RedirectToAction("List");
        }

    }
}