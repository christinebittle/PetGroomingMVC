using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetGrooming.Models.ViewModels
{
    public class UpdateBooking
    {
        //the original booking
        public virtual GroomBooking Booking { get; set; }
        //provide a list of pets
        public virtual List<Pet> Pets { get; set; }
        //provide a list of owners
        public virtual List<Owner> Owners { get; set; }
        //provide a list of groomers
        public virtual List<Groomer> Groomers { get; set; }
        //provide a checkbox list of services
        public virtual List<GroomService> Services { get; set; }
    }
}