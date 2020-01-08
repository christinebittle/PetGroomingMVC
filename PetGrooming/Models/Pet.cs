using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Install  entity framework 6 on Tools > Manage Nuget Packages > Microsoft Entity Framework (ver 6.4)
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace PetGrooming.Models
{
    public class Pet
    {
        /*What defines a pet?*/
        /*For now, we'll use id, species, name, weight*/
        [Key]
        public int PetID { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public string Species { get; set; }
    }
}