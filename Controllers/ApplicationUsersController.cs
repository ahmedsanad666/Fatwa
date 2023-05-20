using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebOS.AuxiliaryClasses;
using WebOS.Data;
using WebOS.Models;
using X.PagedList;

namespace WebOS.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signinmanager;
        private UserManager<ApplicationUser> _usermanager;
        private IWebHostEnvironment _environment;
        private IEmailSender _emailSender;

        public ApplicationUsersController(ApplicationDbContext context, SignInManager<ApplicationUser> signinmamger, UserManager<ApplicationUser> usermanager, IWebHostEnvironment environment, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _environment = environment;
            _usermanager = usermanager;
            _signinmanager = signinmamger;
            _context = context;
        }
        public async Task<IActionResult> ControlPanel(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users
            .SingleOrDefaultAsync(m => m.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            //await _signInManager.SignInAsync(user, isPersistent: false);
            await _signinmanager.SignInAsync(applicationUser, isPersistent: true);
            //if (result.IsCompletedSuccessfully)
            //{

            //  return RedirectToLocal(returnUrl);
            return RedirectToAction("Index", "Home");
            //}

            //   return View(applicationUser);
        }

        [Authorize(Roles ="Admins")]
        public IActionResult Control()
        {

            return View();
        }

        public async Task<IActionResult> Last100()
        {
            var last100 = _context.ApplicationUsers.OrderByDescending(a => a.RegistrationDate).Take(100);
            return View(await last100.ToListAsync());
        }

        public async Task<IActionResult> FastSearch(string keyword)
        {
            ViewData["keyword"] = keyword;
            var users = _context.ApplicationUsers.Where(a => a.ArName.Contains(keyword) || a.Id.Contains(keyword) || a.EnName.Contains(keyword) || a.Email.Contains(keyword) || a.PhoneNumber.Contains(keyword) || a.SecondEmail.Contains(keyword)).Include(a => a.Country).Include(a => a.City);
            return View(await users.ToListAsync());
        }

        public IActionResult Details(string id)
        {
            ApplicationUserViewModel applicationUserVM = new ApplicationUserViewModel()
            {
                ApplicationUser = _context.ApplicationUsers.SingleOrDefault(a => a.Id == id),

            };

            return View(applicationUserVM);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _usermanager.Users
                .Include(c => c.Country)
                .Include(c => c.City)
                .Include(a => a.ReferredBy)
                .SingleOrDefaultAsync(u => u.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.City.Where(c => c.CountryId == applicationUser.CountryId), "Id", "ArCityName", applicationUser.CityId);
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "ArCountryName", applicationUser.CountryId);
            ViewData["ReferredById"] = new SelectList(_context.ApplicationUsers.Where(a => a.Id == applicationUser.ReferredById), "Id", "Id", applicationUser.ReferredById);

            return View(applicationUser);
        }

        [Authorize(Roles = RoleName.Admins)]
        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUsers.Any(e => e.Id == id);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.Admins)]
        public async Task<IActionResult> Edit(string id, [Bind("ReferredById,ArName,EnName,OtherNames,DateofBirth,Gender,SecondEmail,RegDate,WebOS,WebOSDate,Status,UILanguage,ProfileImage,FeaturedImage,CVURL,Summary,ContactMeDetail,CountryId,CityId,UniversityId,FacultyId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,Visitors,ImageProfile")] ApplicationUser applicationUser, IFormFile myfile)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var x = applicationUser.ImageProfile;
                    ApplicationUser thisUser = _usermanager.FindByIdAsync(applicationUser.Id).Result;
                    thisUser.UserName = applicationUser.UserName;
                    thisUser.Email = applicationUser.Email;
                    thisUser.ImageProfile = await UserFile.UploadeNewImageAsync(applicationUser.ImageProfile,
                    myfile, _environment.WebRootPath, Properties.Resources.Images, 400, 300);
                    //thisUser.RegDate = applicationUser.RegDate; // because it is desabled in the view
                    //thisUser.WebOS = applicationUser.WebOS; // because it is desabled in the view
                    //thisUser.WebOSDate = applicationUser.WebOSDate; // because it is desabled in the view
                    //thisUser.ReferredById = applicationUser.ReferredById; // because it is desabled in the view
                    thisUser.Status = applicationUser.Status;
                    thisUser.SecondEmail = applicationUser.SecondEmail;
                    thisUser.PhoneNumber = applicationUser.PhoneNumber;
                    thisUser.ArName = applicationUser.ArName;
                    thisUser.EnName = applicationUser.EnName;
                    thisUser.DateofBirth = applicationUser.DateofBirth;
                    thisUser.Gender = applicationUser.Gender;
                    thisUser.UILanguage = applicationUser.UILanguage;
                    thisUser.CountryId = applicationUser.CountryId;
                    thisUser.CityId = applicationUser.CityId;

                    await _usermanager.UpdateAsync(thisUser);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "ApplicationUsers", new { id });
            }
            ViewData["CityId"] = new SelectList(_context.City.Where(c => c.CountryId == applicationUser.CountryId), "Id", "ArCityName", applicationUser.CityId);
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "ArCountryName", applicationUser.CountryId);
            ViewData["ReferredById"] = new SelectList(_context.ApplicationUsers, "Id", "Id", applicationUser.ReferredById);

            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        [Authorize(Roles = RoleName.Admins)]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUsers
                .Include(c => c.Country)
                .Include(c => c.City)
                .Include(a => a.ReferredBy)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.Admins)]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _context.ApplicationUsers.SingleOrDefaultAsync(m => m.Id == id);

            //_context.Update(ApplicationUsers);
            _context.ApplicationUsers.Remove(applicationUser);

            UserFile.DeleteOldImage(_environment.WebRootPath, "ProfileImage", applicationUser.ImageProfile);



            //-----------------------------------email content-----------------------------------------
            //BackgroundJob.Schedule(() => _emailSender.SendEmailAsync(applicationUser.Email, "جاري حذف حسابكم بالكامل في منصة أُريد", Welcome.ToString() + content2.ToString() + Footer.ToString()), TimeSpan.FromSeconds(10));
            await _emailSender.SendEmailAsync(
                applicationUser.Email, "حذف حساب في النظام", "تم حذف حسابكم بالكامل في النظام");
            //-----------------------------------email content-----------------------------------------

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index(int? page, string keyword)
        {
            ViewData["keyword"] = keyword ?? null;
            List<ApplicationUser> users = new List<ApplicationUser>();
            var numberofpage = page ?? 1;
            if (keyword == null)
            {
                users = _context.ApplicationUsers.Include(a => a.Country).Include(a => a.City).Include(a => a.Nationality).ToList();
            }
            else
            {
                users = _context.ApplicationUsers.Include(a => a.Country).Include(a => a.City).Include(a => a.Nationality).Where(a => a.ArName.Contains(keyword) || a.Email.Contains(keyword) || a.Id.Contains(keyword)).ToList();
            }
            var onepageofusers = users.ToPagedList(numberofpage, 50);
            ViewBag.onepageofusers = onepageofusers;
            return View();
        }


    }
}
