using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeToIt.Data;
using BikeToIt.Models;
using BikeToIt.ViewModels;
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

        // GET: /<controller>/
        public IActionResult Index()
        {
            
            List<string> states = context.Trails.OrderBy(s => s.State).Select(s => s.State).Distinct().ToList();
            List<City> cities = context.Cities.OrderBy(c => c.Name).ToList();
            List<DestinationCategory> categories = context.DestinationCategories.ToList();
            SearchTrailViewModel viewModel = new SearchTrailViewModel(states, cities, categories);
            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Results(SearchTrailViewModel viewModel, string state, string city, string fooddrink, string park, string shop, string camping, string other)
        {

            List<Trail> allTrails = context.Trails
                .Include(t => t.Destinations)
                .ToList();
            List<TrailCity> trailCities = context.TrailCity
                .Include(t => t.Trail)
                .Include(t => t.City).ToList();
            List<Destination> destinations = context.Destinations.ToList();

            List<Trail> selectedTrails = new List<Trail>().ToList();

            Console.WriteLine(state);
            Console.WriteLine(state);
            //if nothing is entered in search, return all results
            if (string.IsNullOrEmpty(viewModel.Name) && string.IsNullOrEmpty(state) && string.IsNullOrEmpty(city) &&
                (viewModel.Distance <= 0) && string.IsNullOrEmpty(viewModel.SurfaceType) && string.IsNullOrEmpty(fooddrink) && string.IsNullOrEmpty(park)
                && string.IsNullOrEmpty(shop) && string.IsNullOrEmpty(camping) && string.IsNullOrEmpty(other))
            {
                selectedTrails = allTrails;
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
                if (viewModel.State != null)
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
                    else
                    {
                        foreach (Trail trail in allTrails)
                        {
                            if (trail.State == state)
                            {
                                selectedTrails.Add(trail);
                            }
                        }
                    }

                }
                //city search
                if (city != null)
                {

                    if (selectedTrails.Count > 0)
                    {
                        foreach (Trail trail in selectedTrails.ToList())
                        {
                            List<string> cities = new List<string>();

                            foreach (TrailCity cityPair in trailCities)
                            {

                                if (cityPair.TrailId == trail.Id)
                                {
                                    cities.Add(cityPair.CityId.ToString());

                                }

                            }

                            if (!cities.Contains(city))
                            {
                                selectedTrails.Remove(trail);
                            }

                        }
                    }
                    else
                    {
                        foreach (Trail trail in allTrails)
                        {
                            foreach (TrailCity cityPair in trailCities)
                            {
                                if (cityPair.TrailId == trail.Id && cityPair.CityId.ToString() == city)
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
                    else
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
                    else
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
                //food and drink search
                if (fooddrink == "on")
                {
                    if (selectedTrails.Count > 0)
                    {
                        foreach (Trail trail in selectedTrails.ToList())
                        {
                            List<int> cats = new List<int>();
                            foreach (Destination d in destinations)
                            {
                                if (trail.Id == d.TrailId)
                                {
                                    cats.Add(d.CategoryId);

                                }

                            }
                            if (!cats.Contains(1))
                            {
                                selectedTrails.Remove(trail);
                            }
                        }

                    }
                    else
                    {
                        foreach (Trail trail in allTrails)
                        {
                            foreach (Destination d in destinations)
                            {
                                if (trail.Id == d.TrailId && d.CategoryId == 1 && !selectedTrails.Contains(trail))
                                {
                                    selectedTrails.Add(trail);
                                }
                            }
                        }
                    }

                }
                //park search
                if (park == "on")
                {
                    if (selectedTrails.Count > 0)
                    {
                        foreach (Trail trail in selectedTrails.ToList())
                        {
                            List<int> cats = new List<int>();
                            foreach (Destination d in destinations)
                            {
                                if (trail.Id == d.TrailId)
                                {
                                    cats.Add(d.CategoryId);

                                }

                            }
                            if (!cats.Contains(2))
                            {
                                selectedTrails.Remove(trail);
                            }
                        }

                    }
                    else
                    {
                        foreach (Trail trail in allTrails)
                        {
                            foreach (Destination d in destinations)
                            {
                                if (trail.Id == d.TrailId && d.CategoryId == 2 && !selectedTrails.Contains(trail))
                                {
                                    selectedTrails.Add(trail);
                                }
                            }
                        }
                    }

                }

                //shop search
                if (shop == "on")
                {
                    if (selectedTrails.Count > 0)
                    {
                        foreach (Trail trail in selectedTrails.ToList())
                        {
                            List<int> cats = new List<int>();
                            foreach (Destination d in destinations)
                            {
                                if (trail.Id == d.TrailId)
                                {
                                    cats.Add(d.CategoryId);
                                    
                                }
  
                            }
                            if (!cats.Contains(3))
                            {
                                selectedTrails.Remove(trail);
                            }
                        }

                    }
                    else
                    {
                        foreach (Trail trail in allTrails)
                        {
                            foreach (Destination d in destinations)
                            {
                                if (trail.Id == d.TrailId && d.CategoryId == 3&& !selectedTrails.Contains(trail))
                                {
                                    selectedTrails.Add(trail);
                                }
                            }
                        }
                    }

                }

                //camping search
                if (camping == "on")
                {
                    if (selectedTrails.Count > 0)
                    {
                        foreach (Trail trail in selectedTrails.ToList())
                        {
                            List<int> cats = new List<int>();
                            foreach (Destination d in destinations)
                            {
                                if (trail.Id == d.TrailId)
                                {
                                    cats.Add(d.CategoryId);

                                }

                            }
                            if (!cats.Contains(4))
                            {
                                selectedTrails.Remove(trail);
                            }
                        }

                    }
                    else
                    {
                        foreach (Trail trail in allTrails)
                        {
                            foreach (Destination d in destinations)
                            {
                                if (trail.Id == d.TrailId && d.CategoryId == 4 && !selectedTrails.Contains(trail))
                                {
                                    selectedTrails.Add(trail);
                                }
                            }
                        }
                    }

                }
                //other search
                if (other == "on")
                {
                    if (selectedTrails.Count > 0)
                    {
                        foreach (Trail trail in selectedTrails.ToList())
                        {
                            List<int> cats = new List<int>();
                            foreach (Destination d in destinations)
                            {
                                if (trail.Id == d.TrailId)
                                {
                                    cats.Add(d.CategoryId);

                                }

                            }
                            if (!cats.Contains(5))
                            {
                                selectedTrails.Remove(trail);
                            }
                        }

                    }
                    else
                    {
                        foreach (Trail trail in allTrails)
                        {
                            foreach (Destination d in destinations)
                            {
                                if (trail.Id == d.TrailId && d.CategoryId == 5 && !selectedTrails.Contains(trail))
                                {
                                    selectedTrails.Add(trail);
                                }
                            }
                        }
                    }

                }



            }
            List<City> allCities = context.Cities.ToList();
            List<int> selectedCities = new List<int>();
            List<City> resultCities = new List<City>();


            foreach (Trail trail in selectedTrails.ToList())
            {

                foreach (TrailCity cityPair in trailCities)
                {

                    if (cityPair.TrailId == trail.Id && !selectedCities.Contains(cityPair.CityId))
                    {
                        selectedCities.Add(cityPair.CityId);

                    }

                }

            }

            foreach(int c in selectedCities)
            {
                foreach(City cty in allCities)
                {
                    if (c == cty.Id)
                    {
                        resultCities.Add(cty);
                    }
                }
            }

            
                

            SearchResultsViewModel results = new SearchResultsViewModel(selectedTrails, trailCities);


            return View(results);
        }


        public IActionResult Detail(int id)
        {
            Trail theTrail = context.Trails
                .Single(t => t.Id == id);


            List<Destination> destinations = context.Destinations
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
