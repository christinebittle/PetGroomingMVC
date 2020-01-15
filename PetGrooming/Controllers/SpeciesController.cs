using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetGrooming.Controllers
{
    public class SpeciesController : Controller
    {
        // GET: Species
        public ActionResult Index()
        {
            return View();
        }
    }
}