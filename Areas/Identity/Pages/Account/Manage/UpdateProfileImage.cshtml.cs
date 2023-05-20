using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebOS.AuxiliaryClasses;
using WebOS.Data;
using WebOS.Models;

namespace WebOS.Areas.Identity.Pages.Account.Manage
{
    public partial class UpdateProfileImageModel:PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _environment;


        public UpdateProfileImageModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _environment = environment;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [StringLength(500)]
        [Display(Name = "الصورة الشخصية")]
        public string ProfileImage { get; set; }
       
        [StringLength(500)]
        [Display(Name = "الصورة الشخصية")]
        //public IFormFile ProfileImageFile { get; set; }
        public string StatusMessage { get; set; }

        private async Task LoadAsync(ApplicationUser user)
        {
            ProfileImage = user.ImageProfile;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile myfile,string ProfileImage)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            if (myfile != null)
            {
                user.ImageProfile = await UserFile.UploadeNewImageAsync(user.ImageProfile,
myfile, _environment.WebRootPath, Properties.Resources.Images, 400, 300);
                await _userManager.UpdateAsync(user);
                StatusMessage = "تم تغيير الصورة";
            }
            StatusMessage = "لم يتم تغيير الصورة";

            _context.SaveChanges();

            //StatusMessage = "Your Image is unchanged.";
            return RedirectToPage();
        }

    }
}
