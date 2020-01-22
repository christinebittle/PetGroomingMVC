using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Install  entity framework 6 on Tools > Manage Nuget Packages > Microsoft Entity Framework (ver 6.4)
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming.Models
{
    public class Pet
    {
        /*
            A pet is an animal that receives the grooming that the owner pays for
            Some things that describe a pet:
                - Name
                - Weight
                - Species
                - Color
                - Special Notes

            A Pet must reference a Species
        */
        [Key]
        public int PetID { get; set; }
        public string PetName { get; set; }
        //weight is in kilograms (kg)
        public double Weight { get; set; }
        public string Color { get; set; }
        public string Notes { get; set; }



        //Representing the Many in (One species to Many Pets)
        
        public int SpeciesID { get; set; }
        [ForeignKey("SpeciesID")]
        public virtual Species Species { get; set; }
    }
}