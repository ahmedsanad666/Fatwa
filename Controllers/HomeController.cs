using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using WebOS.Data;
using WebOS.Models;
using Newtonsoft.Json;
using System.Net.Http;
using WebOS.Migrations;

namespace WebOS.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _usermanager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
            _logger = logger;
        }

        public JsonResult GetCitiesList(int countryid)
        {
            var cities = new SelectList(_context.City.Where(c => c.CountryId == countryid), "Id", "ArCityName");
            return Json(cities);
        }



        //notifications


        //messages

        public IActionResult GetMyViewCompMessage(string uid)
        {
            return ViewComponent("Message", new { id = uid });//it will call Follower.cs InvokeAsync, and pass id to it.
        }





        public async Task<IActionResult> Index(string keyword)
        {
            if (keyword == null) {

                List<Hadith> allHadith = new();

              Root root = new Root();

                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://api.hadith.gading.dev/books/bukhari?range=300-500"; // Replace with your API endpoint

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        root = JsonConvert.DeserializeObject<Root>(json);
                        
                      allHadith = root.data.hadiths.ToList();
                       
                     
                    }
                    else
                    {
                        // Handle the API error here
                        Console.WriteLine("error to get all hadith");
                    }
                }


                Homevm HomeViewModel = new Homevm()
                {
                    
                    allHadith = allHadith,
                    Fatwas = _context.Fatwa.Include(f => f.Category).Include(f => f.Scholar).Take(20),
                    Categories = _context.Category,
                };

                //var applicationDbContext = _context.Fatwa.Include(f => f.Category).Include(f => f.Scholar);
                return View(HomeViewModel);
            }
            else
            {
                var applicationDbContext = _context.Fatwa.Where(a => a.Title.Contains(keyword) || a.Question.Contains(keyword) || a.Answer.Contains(keyword) || a.Tags.Contains(keyword) || a.Scholar.Name.Contains(keyword)).Include(f => f.Category).Include(f => f.Scholar);
                if (applicationDbContext.Count() > 0)
                {
                    return RedirectToAction("Search", "Fatwas", new { keyword = keyword });

                }

                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
          
            

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetHadithData()
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://api.hadith.gading.dev/books/bukhari?range=300-500"; // Replace with your API endpoint

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Hadith data = JsonConvert.DeserializeObject<Hadith>(json);


                    return View(data);
                }
                else
                {
                    // Handle the API error here
                    return View("Error");
                }
            }
        }



    }
}
