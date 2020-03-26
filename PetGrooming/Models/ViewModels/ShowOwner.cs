using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetGrooming.Models.ViewModels
{
    public class ShowOwner
    {

        //one individual owner
        public virtual Owner owner {get;set;}
        //a list for every pet they own
        public List<Pet> pets { get; set; }

        //a list of all grooms billed to this owner
        public List<GroomBooking> billedgrooms { get; set; }

        //a SEPARATE list for representing the ADD of an owner to a pet,
        //i.e. show a dropdownlist of all pets, with cta "Add Pet" on Show Owner etc.
        public List<Pet> all_pets { get; set; }

    }
}