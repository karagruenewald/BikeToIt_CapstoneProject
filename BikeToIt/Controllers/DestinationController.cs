using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BikeToIt.Data;
using BikeToIt.Models;
using BikeToIt.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BikeToIt.Controllers
{
    public class DestinationController : Controller
    {
        private TrailDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;


        public DestinationController(TrailDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            context = dbContext;
            webHostEnvironment = hostEnvironment;
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
            string uniqueFileName = UploadedFile(viewModel);

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
                    TrailId = viewModel.TrailId,
                    Image = uniqueFileName

                };

                context.Destinations.Add(newDestination);
                context.SaveChanges();

                int id = newDestination.Id;

                return Redirect("/destination/detail/"+id);
            }

            //repopulate SelectListItems
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

        //helper function for images
        private string UploadedFile(AddDestinationViewModel model)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }

            }
            return uniqueFileName;
        }

        public IActionResult Edit(int id)
        {
            Destination d = context.Destinations.Find(id);
            List<DestinationCategory> dc = context.DestinationCategories.ToList();
            List<Trail> t = context.Trails.ToList();

            EditDestinationViewModel viewModel = new EditDestinationViewModel(d, dc, t);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditDestinationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Destination destination = context.Destinations.Find(viewModel.Id);

                destination.Name = viewModel.Name;
                destination.Street = viewModel.Street;
                destination.City = viewModel.City;
                destination.State = viewModel.State;
                destination.Zipcode = viewModel.Zipcode;
                destination.Description = viewModel.Description;
                destination.Website = viewModel.Website;
                destination.OutdoorSeating = viewModel.OutdoorSeating;
                destination.BikeRacks = viewModel.BikeRacks;
                destination.Restrooms = viewModel.Restrooms;
                destination.Playground = viewModel.Playground;

                if(viewModel.Image != null)
                {
                    string uniqueFileName = NewUploadedFile(viewModel);
                    destination.Image = uniqueFileName;
                }
                else
                {
                    destination.Image = destination.Image;
                }

            }
            context.SaveChanges();
            return Redirect("/Destination/Detail/" + viewModel.Id);


        }

        private string NewUploadedFile(EditDestinationViewModel model)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }

            }
            return uniqueFileName;
        }

    }
}
