using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeToIt.Data;
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
            return View();
        }
    }
}
