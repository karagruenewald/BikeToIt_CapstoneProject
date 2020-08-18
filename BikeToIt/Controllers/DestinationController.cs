using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BikeToIt.Data;
using BikeToIt.Models;
using BikeToIt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BikeToIt.Controllers
{

    public class DestinationController : Controller
    {
        private TrailDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;


        public DestinationController(TrailDbContext dbContext, IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager)
        {
            context = dbContext;
            webHostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            List<Destination> alldestinations = context.Destinations
                .Include(t => t.Trail)
                .Include(t => t.Category)
                .OrderBy(t => t.Id)
                .ToList();

            return View(alldestinations);
        }

        [AllowAnonymous]
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

        [Authorize]
        public IActionResult Add()
        {

            List<DestinationCategory> categories = context.DestinationCategories.ToList();
            List<Trail> trails = context.Trails.OrderBy(t => t.Name).ToList();

            AddDestinationViewModel destination = new AddDestinationViewModel(categories, trails);

            return View(destination);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(AddDestinationViewModel viewModel)
        {
            string uniqueFileName = UploadedFile(viewModel);



            if (ModelState.IsValid && User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);

                DestinationCategory category = context.DestinationCategories.Find(viewModel.CategoryId);
                Trail trail = context.Trails.Find(viewModel.TrailId);

                if (!viewModel.Website.StartsWith("http"))
                {
                    viewModel.Website = "https://"+viewModel.Website;
                }


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
                    Image = uniqueFileName,
                    UserId = userId

                };

                context.Destinations.Add(newDestination);
                context.SaveChanges();

                int id = newDestination.Id;

                return Redirect("/destination/detail/" + id);
            }
            else if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);

                DestinationCategory category = context.DestinationCategories.Find(viewModel.CategoryId);
                Trail trail = context.Trails.Find(viewModel.TrailId);

                if (!viewModel.Website.StartsWith("http"))
                {
                    viewModel.Website = "https://" + viewModel.Website;
                }

                UserDestination newUserDestination = new UserDestination
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
                    Image = uniqueFileName,
                    UserId = userId

                };

                context.UserDestinations.Add(newUserDestination);
                context.SaveChanges();


                return Redirect("/Destination/UserDestinations/");

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
            List<SelectListItem> trailList = new List<SelectListItem>();

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
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }

            }
            return uniqueFileName;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Destination d = context.Destinations.Find(id);
            List<DestinationCategory> dc = context.DestinationCategories.ToList();
            List<Trail> t = context.Trails.ToList();

            EditDestinationViewModel viewModel = new EditDestinationViewModel(d, dc, t);

            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
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
                destination.CategoryId = viewModel.CategoryId;
                destination.TrailId = viewModel.TrailId;

                if (viewModel.Image != null)
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
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {

            Destination destination = context.Destinations
                .Include(d => d.Category)
                .Single(d => d.Id == id);

            return View(destination);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(Destination model)
        {

            Destination destination = context.Destinations.Find(model.Id);

            context.Destinations.Remove(destination);
            context.SaveChanges();

            return Redirect("/Destination");
        }

        public IActionResult UserDestinations()
        {
            var userId = _userManager.GetUserId(User);

            List<UserDestination> userDestinations = context.UserDestinations
                .Include(d => d.Trail)
                .Include(d => d.Category)
                .Where(d => d.UserId == userId)
                .ToList();
            List<Destination> userApprovedDestinations = context.Destinations
                .Include(d => d.Trail)
                .Include(d => d.Category)
                .Where(d => d.UserId == userId)
                .ToList();

            AllUserDestinations allUserDestinations = new AllUserDestinations(userDestinations, userApprovedDestinations);
            return View(allUserDestinations);
        }


        [Authorize(Roles = "Admin")]
        //[Route("Destination/Approve")]
        public IActionResult NeedingAdminApproval()
        {
            List<UserDestination> userDestinationsNeedingApproval = context.UserDestinations
                .Include(d => d.Trail)
                .Include(d => d.Category)
                .ToList();

            return View(userDestinationsNeedingApproval);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult NeedingAdminApproval(int id)
        {

            UserDestination destinationSelected = context.UserDestinations
                .Include(d => d.Trail)
                .Include(d => d.Category)
                .Single(d => d.Id == id);

            DestinationCategory category = context.DestinationCategories.Find(destinationSelected.CategoryId);
            Trail trail = context.Trails.Find(destinationSelected.TrailId);

            Destination newDestination = new Destination
            {
                Name = destinationSelected.Name,
                Street = destinationSelected.Street,
                City = destinationSelected.City,
                State = destinationSelected.State,
                Zipcode = destinationSelected.Zipcode,
                Description = destinationSelected.Description,
                Website = destinationSelected.Website,
                OutdoorSeating = destinationSelected.OutdoorSeating,
                BikeRacks = destinationSelected.BikeRacks,
                Restrooms = destinationSelected.Restrooms,
                Playground = destinationSelected.Playground,
                Category = category,
                Trail = trail,
                CategoryId = destinationSelected.CategoryId,
                TrailId = destinationSelected.TrailId,
                Image = destinationSelected.Image,
                UserId = destinationSelected.UserId
            };

            context.Destinations.Add(newDestination);
            context.UserDestinations.Remove(destinationSelected);
            context.SaveChanges();


            return Redirect("/Destination");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditUserDestination(int id)
        {
            UserDestination d = context.UserDestinations.Find(id);
            List<DestinationCategory> dc = context.DestinationCategories.ToList();
            List<Trail> t = context.Trails.ToList();

            EditUserDestinationViewModel viewModel = new EditUserDestinationViewModel(d, dc, t);

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditUserDestination(EditUserDestinationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                UserDestination destination = context.UserDestinations.Find(viewModel.Id);

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
                destination.CategoryId = viewModel.CategoryId;
                destination.TrailId = viewModel.TrailId;

                if (viewModel.Image != null)
                {
                    string uniqueFileName = NewUploadedFileForUserDestination(viewModel);
                    destination.Image = uniqueFileName;
                }
                else
                {
                    destination.Image = destination.Image;
                }
            }
            context.SaveChanges();
            return Redirect("/Destination/NeedingAdminApproval");

        }

        private string NewUploadedFileForUserDestination(EditUserDestinationViewModel model)
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