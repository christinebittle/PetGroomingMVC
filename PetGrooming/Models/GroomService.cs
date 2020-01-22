using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Install  entity framework 6 on Tools > Manage Nuget Packages > Microsoft Entity Framework (ver 6.4)
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace PetGrooming.Models
{
    public class GroomService
    {
        /*
            A GroomService is a type of activity that a Groomer can do for a pet. Some examples might be nail clipping, shampoo, trim, etc.
            
            Some things that describe a GroomService
                - Service Name
                - Cost
                - Duration
         */
         public int GroomServiceID { get; set; }
        public string ServiceName { get; set; }
        //Cost is established as Cents rather than dollars (i.e. 2000c = $20.00)
        //currency is CANADIAN (cad)
        public int ServiceCost { get; set; }
        //Service duration established as minutes (i.e. 90min = 1hour30min)
        public int ServiceDuration { get; set; }
    }
}