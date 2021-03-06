﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeToIt.Data;
using BikeToIt.Models;
using BikeToIt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // lists all trails
        [Authorize(Roles = "Admin")]

        public IActionResult Index()
        {
            List<Trail> allTrails = context.Trails
                .Include(t => t.Destinations)
                .OrderBy(t => t.Id)
                .ToList();

            List<TrailCity> cities = context.TrailCity
                .Include(c => c.City)
                .Include(c => c.Trail)
                .ToList();

            AllTrailsViewModel viewModel = new AllTrailsViewModel(allTrails, cities);

            return View(viewModel);
        }

        //trail search form
        public IActionResult Search()
        {            
            List<string> states = context.Trails.OrderBy(s => s.State).Select(s => s.State).Distinct().ToList();
            List<City> cities = context.Cities.OrderBy(c => c.Name).ToList();
            List<DestinationCategory> categories = context.DestinationCategories.ToList();
            SearchTrailViewModel viewModel = new SearchTrailViewModel(states, cities, categories);

            if (TempData["emptySearch"] != null)
            {
                ViewBag.error = TempData["emptySearch"].ToString();
            }

            return View(viewModel);
        }

        //results of search
        [HttpPost]
        public IActionResult Results(SearchTrailViewModel viewModel, string state)
        {

            List<Trail> allTrails = context.Trails
                .Include(t => t.Destinations)
                .ToList();
            List<TrailCity> trailCities = context.TrailCity
                .Include(t => t.Trail)
                .Include(t => t.City).ToList();
            List<Destination> destinations = context.Destinations.ToList();

            List<Trail> selectedTrails = new List<Trail>().ToList();
            
                       
            //if nothing is entered in search, it redirects back to the search page with error message
            if (string.IsNullOrEmpty(viewModel.Name) && string.IsNullOrEmpty(state) && (viewModel.CityId == 0) &&
                (viewModel.Distance <= 0) && string.IsNullOrEmpty(viewModel.SurfaceType) && (viewModel.FoodDrink == false) && (viewModel.Parks == false)
                && (viewModel.Shops == false) && (viewModel.Camping == false) && (viewModel.Other == false))
            {
                TempData["emptySearch"] = "You must enter some search criteria.";
                return Redirect("Search");
                
            }
            else
            { 
                //name search
                if (viewModel.Name != null)
                {
                    foreach (Trail trail in allTrails)
                    {

                        if (trail.Name.ToLower().Contains(viewModel.Name.ToLower()) && !selectedTrails.Contains(trail))
                        {
                            selectedTrails.Add(trail);
                        }

                    }
                }
                //state search
                if (state != null)
                {                    
   
                            if (selectedTrails.Count > 0)
                            {
                                foreach (Trail trail in selectedTrails.ToList())
                                {
                                    if (trail.State != state)
                                    {
                                        selectedTrails.Remove(trail);
                                    }
                                }

                            }
                            else if (selectedTrails.Count == 0 && viewModel.Name == null)
                            {
                                foreach (Trail trail in allTrails)
                                {

                                    if (trail.State == state && (!selectedTrails.Contains(trail)))
                                    {
                                        selectedTrails.Add(trail);
                                    }

                                }
                            }

                }
                //city search
                if (viewModel.CityId != 0)
                {

                    if (selectedTrails.Count > 0)
                    {
                        foreach (Trail trail in selectedTrails.ToList())
                        {
                            List<int> cities = new List<int>();

                            foreach (TrailCity cityPair in trailCities)
                            {

                                if (cityPair.TrailId == trail.Id)
                                {
                                    cities.Add(cityPair.CityId);

                                }

                            }

                            if (!cities.Contains(viewModel.CityId))
                            {
                                selectedTrails.Remove(trail);
                            }

                        }
                    }
                    else if (selectedTrails.Count == 0 && viewModel.Name == null && state == null)
                    {
                        foreach (Trail trail in allTrails)
                        {
                            foreach (TrailCity cityPair in trailCities)
                            {
                                if (cityPair.TrailId == trail.Id && cityPair.CityId == viewModel.CityId)
                                {
                                    selectedTrails.Add(trail);
                                }

                            }

                        }
                    }

                }
                //distance search
                if (viewModel.Distance > 0)
                {
                    if (selectedTrails.Count > 0)
                    {
                        foreach (Trail trail in selectedTrails.ToList())
                        {
                            if (trail.Distance <= viewModel.Distance)
                            {
                                selectedTrails.Remove(trail);
                            }
                        }
                    }
                    else if (selectedTrails.Count == 0 && viewModel.Name == null && state == null && viewModel.City == null)
                    {
                        foreach (Trail trail in allTrails)
                        {
                            if (trail.Distance >= viewModel.Distance)
                            {
                                selectedTrails.Add(trail);
                            }
                        }
                    }

                }
                //surface type search
                if (viewModel.SurfaceType != null)
                {
                    if (selectedTrails.Count > 0)
                    {
                        foreach (Trail trail in selectedTrails.ToList())
                        {
                            if (!trail.SurfaceType.Contains(viewModel.SurfaceType))
                            {
                                selectedTrails.Remove(trail);
                            }
                        }
                    }
                    else if(selectedTrails.Count == 0 && viewModel.Name == null && state == null && viewModel.City == null && viewModel.Distance == 0)
                    {
                        foreach (Trail trail in allTrails)
                        {
                            if (trail.SurfaceType.Contains(viewModel.SurfaceType))
                            {
                                selectedTrails.Add(trail);
                            }
                        }
                    }

                }
                //destination category search
                List<int> categories = new List<int>();

                if(viewModel.FoodDrink == true)
                {
                    categories.Add(1);
                }
                if (viewModel.Parks == true)
                {
                    categories.Add(2);
                }
                if (viewModel.Shops == true)
                {
                    categories.Add(3);
                }
                if (viewModel.Camping == true)
                {
                    categories.Add(4);
                }
                if (viewModel.Other == true)
                {
                    categories.Add(5);
                }

                if (categories.Count > 0)
                {
                    if (selectedTrails.Count > 0)
                    {
                        foreach (Trail trail in selectedTrails.ToList())
                        {
                            List<int> cats = new List<int>();
                            foreach (Destination d in destinations)
                            {
                                if (trail.Id == d.TrailId && !cats.Contains(d.CategoryId))
                                {
                                    cats.Add(d.CategoryId);

                                }

                            } // if trail destination categories list(cats) doesn't contain categories checked(categories), remove trail
                            if (!categories.All(i => cats.Contains(i)))
                            {
                                selectedTrails.Remove(trail);
                            }
                        }
                    }
                    else if (selectedTrails.Count == 0 && viewModel.Name == null && state == null && viewModel.City == null && viewModel.Distance == 0 && viewModel.SurfaceType == null)
                    {
                        foreach (Trail trail in allTrails)
                        {
                            List<int> cats = new List<int>();

                            foreach (Destination d in destinations)
                            {
                                if (trail.Id == d.TrailId && !cats.Contains(d.CategoryId))
                                {
                                    cats.Add(d.CategoryId);

                                }
                            }
                            // if trail destination categories contain categories checked, add trail
                            if (categories.All(i => cats.Contains(i)))
                            {
                                selectedTrails.Add(trail);
                            }

                        }
                    }

                }

            }
            

            List<Trail> selectedTrailsAsc = selectedTrails.OrderBy(t => t.Name).ToList();
            List<TrailCity> trailCitiesAsc = trailCities.OrderBy(c => c.City.Name).ToList();
                           
            SearchResultsViewModel results = new SearchResultsViewModel(selectedTrailsAsc, trailCitiesAsc);
            return View(results);
        }

        //lists details about specific trail
        public IActionResult Detail(int id)
        {
            Trail theTrail = context.Trails
                .Single(t => t.Id == id);


            List<Destination> destinations = context.Destinations
                .Include(t => t.Category)
                .Where(t => t.TrailId == id)
                .ToList();

            List<TrailCity> theCities = context.TrailCity
                .Where(t => t.TrailId == id)
                .Include(t => t.City)
                .ToList();

            TrailDetailViewModel viewModel = new TrailDetailViewModel(theTrail, destinations, theCities);

            return View(viewModel);
        }
    }
}
