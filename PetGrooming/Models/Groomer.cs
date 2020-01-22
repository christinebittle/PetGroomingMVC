using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Install  entity framework 6 on Tools > Manage Nuget Packages > Microsoft Entity Framework (ver 6.4)
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace PetGrooming.Models
{
    public class Groomer
    {
        [Key]
        public int GroomerID { get; set; }
        public string GroomerFName { get; set; }
        public string GroomerLName { get; set; }
        public DateTime GroomerDOB { get; set; }

        //Described in CENTS per hour (i.e. $25/hr = 2500cents/hr)
        public int GroomerRate { get; set; }
        /* 
            A groomer is someone who is employed to groom pets
            Some things that describe a groomer
                - First Name
                - Last Name
                - Date of Birth
                - Phone Number
                - Hourly Rate

            A booking must reference to a groomer
        */
    }
}