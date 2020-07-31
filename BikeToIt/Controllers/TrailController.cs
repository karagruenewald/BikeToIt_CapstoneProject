using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeToIt.Data;
using BikeToIt.Models;
using BikeToIt.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BikeToIt.Controllers
{
    public class TrailController : Controller
    {

        private TrailDbContext context;

        public TrailController(TrailDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<string> states = context.Trails.Select(s => s.State).Distinct().ToList();
            List<City> cities = context.Cities.ToList();
            SearchEventViewModel viewModel = new SearchEventViewModel(states, cities);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Results()
        {
            List<Trail> allTrails = context.Trails.ToList();
            List<Trail> selectedTrails;

            return null;
        }
    }
}
