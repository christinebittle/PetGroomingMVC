using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetGrooming.Models.ViewModels
{
    public class UpdatePet
    {
        //when we need to update a pet
        //we need the pet info as well as a list of species

        public Pet Pet { get; set; }
        public List<Species> Species { get; set; }
    }
}