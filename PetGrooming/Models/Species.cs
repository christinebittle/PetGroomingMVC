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
    public class Species
    {

        [Key]
        public int SpeciesID { get; set; }

        public string Name { get; set; }

        //Representing the "Many" in (One Species to many Pets)
        public ICollection<Pet> Pets { get; set; }
    }
}