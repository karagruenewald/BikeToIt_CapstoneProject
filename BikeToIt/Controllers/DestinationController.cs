using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeToIt.Data;
using BikeToIt.Models;
using BikeToIt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BikeToIt.Controllers
{
    public class DestinationController : Controller
    {
        private TrailDbContext context;


        public DestinationController(TrailDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            Destination destination = context.Destinations
                .Single(t => t.Id == id);

            DestinationCategory categories = context.DestinationCategories
                .Single(d => d.Id == destination.CategoryId);

            Trail trail = context.Trails
                .Single(t => t.Id == destination.TrailId);
                

            DestinationDetailViewModel theDestination = new DestinationDetailViewModel(destination, categories, trail);
            return View(theDestination);
                
                
        }

        public IActionResult Add()
        {

            List<DestinationCategory> categories = context.DestinationCategories.ToList();
            List<Trail> trails = context.Trails.ToList();

            AddDestinationViewModel destination = new AddDestinationViewModel(categories, trails);

            return View(destination);
        }

        [HttpPost]
        public IActionResult Add(AddDestinationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                DestinationCategory category = context.DestinationCategories.Find(viewModel.CategoryId);
                Trail trail = context.Trails.Find(viewModel.TrailId);


                Destination newDestination = new Destination
                {
                    Name = viewModel.Name,
                    Street = viewModel.Street,
                    City = viewModel.City,
                    State = viewModel.State,
                    Zipcode = viewModel.Zipcode,
                    Description = viewModel.Description,
                    Website = viewModel.Website,
                    OutdoorSeating = viewModel.OutdoorSeating,
                    BikeRacks = viewModel.BikeRacks,
                    Restrooms = viewModel.Restrooms,
                    Playground = viewModel.Playground,
                    Category = category,
                    Trail = trail,
                    CategoryId = viewModel.CategoryId,
                    TrailId = viewModel.TrailId

                };

                context.Destinations.Add(newDestination);
                context.SaveChanges();

                int id = newDestination.Id;

                return Redirect("/destination/detail/"+id);
            }

            List<DestinationCategory> categories = context.DestinationCategories.ToList();
            List<Trail> trails = context.Trails.ToList();

            List<SelectListItem> categoryList = new List<SelectListItem>();

            foreach (var c in categories)
            {
                categoryList.Add(new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
            }
            List<SelectListItem>trailList = new List<SelectListItem>();

            foreach (var t in trails)
            {
                trailList.Add(new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                });

            }

            viewModel.Category = categoryList;
            viewModel.Trail = trailList;


            return View(viewModel);
        }

    }
}
