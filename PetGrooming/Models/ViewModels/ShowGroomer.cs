using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetGrooming.Models.ViewModels
{
    public class ShowGroomer
    {

        //The groomer entity itself
        public virtual Groomer groomer { get; set; }
        //every booking that groomer has
        public List<GroomBooking> bookings { get; set; }
    }
}