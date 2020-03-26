using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetGrooming.Models.ViewModels
{
    public class ShowPet
    {
        //information about an individual pet
        public virtual Pet pet { get; set; }

        //information about multiple owners
        public List<Owner> owners { get; set; }

        //information about grooms booked for this pet
        public List<GroomBooking> bookedgrooms { get; set; }
    }
}