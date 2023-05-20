using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebOS.AuxiliaryClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebOS.Data;
using WebOS.Models;
using static WebOS.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;

namespace WebOS.Areas.Identity.Pages.Account.Manage
{

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _environment;


        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _environment = environment;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [Display(Name = "إسم المستخدم")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Key]
            public string Id { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "البريد الالكتروني")]
            public string Email { get; set; }

            [Display(Name = "نوع الحساب")]
            [Required(ErrorMessage = "RequiredFieldError")]
            public AccountType AccountType { get; set; }

            [StringLength(4)]
            [Display(Name = "Support Pin")]
            public string SupportPin { get; set; }

            [Display(Name = "الاشتراك بالقائمة البريدية")]
            public bool IsNewsletter { get; set; }

            [Phone]
            [Display(Name = "رقم النقال")]
            public string PhoneNumber { get; set; }

            [Display(Name = "رقم الحساب")]
            public string AccountNumber { get; set; }

            [Display(Name = "الاسم العربي")]
            [Required(ErrorMessage = "هذا الحقل ضروري")]
            [StringLength(50, ErrorMessage = "الاسم قصير", MinimumLength = 6)]
            public string ArName { get; set; }

            [Display(Name = "الاسم بالانجليزي")]
            [Required(ErrorMessage ="هذا الحقل ضروري")]
            [StringLength(50, ErrorMessage = "الاسم يجب ان يكون بين 6 و 50 حرف", MinimumLength = 6 )]
            public string EnName { get; set; }

            [Display(Name = "تاريخ التسجيل")]
            [DataType(DataType.DateTime)]
            [DisplayFormat(DataFormatString = "{0:d}")]
            public DateTime RegistrationDate { get; set; }

            [Display(Name = "الرصيد")]
            public decimal Balance { get; set; }

            [Display(Name = "البريد البديل")]
            [EmailAddress(ErrorMessage = "EmailError")]
            [StringLength(100)]
            public string SecondEmail { get; set; }

            [Display(Name = "نقاط الولاء")]
            public int IncentivePoints { get; set; }

            [StringLength(500)]
            [Display(Name = "الصورة الشخصية")]
            public string ImageProfile { get; set; }

            [Display(Name = "الدولة الأُم")]
            public int NationalityId { get; set; }
            public Country Nationality { get; set; }

            [Display(Name = "الجنسية")]
            [DefaultValue(1)]
            //[Required(ErrorMessage = "RequiredFieldError")]
            public int CountryId { get; set; }
            public Country Country { get; set; }

            [Display(Name = "المدينة")]
            //[Required(ErrorMessage = "RequiredFieldError")]
            [DefaultValue(1)]
            public int CityId { get; set; }
            public City City { get; set; }

            [StringLength(450)]
            [Display(Name = "إنضم بواسطة")]
            public string ReferredById { get; set; }
            public virtual ApplicationUser ReferredBy { get; set; }

            [Display(Name = "تأريح الولادة")]
            [Required(ErrorMessage = "RequiredFieldError")]
            [DataType(DataType.Date)]
            public DateTime DateofBirth { get; set; }

            [Display(Name = "الجنس")]
            [Required(ErrorMessage = "RequiredFieldError")]
            [DefaultValue(GenderType.Unknown)]
            public GenderType Gender { get; set; }

            [Display(Name = "الحالة")]
            [DefaultValue(StatusType.New)]
            public StatusType Status { get; set; }

            [Display(Name = "العمل")]
            [DefaultValue(JobName.Other)]
            public JobName JobName { get; set; }

            [Display(Name = "لغة الواجهة")]
            [Required(ErrorMessage = "RequiredFieldError")]
            public Language UILanguage { get; set; }
        }

        private async Task LoadAsync()
        {
            var user = _context.ApplicationUsers.SingleOrDefault(a => a.Id == _userManager.GetUserId(User));
            
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);


            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };

            var userName = await _userManager.GetUserNameAsync(user);
            var imageprofile = user.ImageProfile;
            var email = await _userManager.GetEmailAsync(user);
            var isnewsletter = user.IsNewsletter;
            var accountnumber = user.Account;
            var arname = user.ArName;
            var enname = user.EnName;
            var registrationdate = user.RegistrationDate;
            var balance = user.Balance;
            var secondaryemail = user.SecondEmail;
            var Id = user.Id;
            var supportpin = user.SupportPin;
            var incentivepoints = user.IncentivePoints;
            var nasionalityid = user.NationalityId;
            var countryid = user.CountryId;
            var cityid = user.CityId;
            var referredbyid = user.ReferredById;
            var dateofbirth = user.DateofBirth;
            var gender = user.Gender;
            var status = user.Status;
            var job = user.JobId;
            var uilang = user.UILanguage;
            Username = userName;
            ViewData["NasionalityId"] = new SelectList(_context.Country, "Id", "ArCountryName", nasionalityid);
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "ArCountryName", countryid);
            ViewData["CityId"] = new SelectList(_context.City.Where(c=>c.CountryId==user.CountryId), "Id", "ArCityName", cityid);

            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
                AccountNumber = accountnumber,
                ArName = arname,
                EnName = enname,
                RegistrationDate = registrationdate,
                IsNewsletter = isnewsletter,
                Balance = balance,
                SecondEmail = secondaryemail,
                SupportPin = supportpin,
                NationalityId = nasionalityid,
                CountryId = countryid,
                CityId = cityid,
                ReferredById = referredbyid,
                DateofBirth = dateofbirth,
                Gender = gender,
                Status = status,
                JobName = job,
                UILanguage = uilang,
                ImageProfile = imageprofile,
                Id = Id,

            };

           var IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile myfile,string birthdate)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //if (!ModelState.IsValid)
            //{
            //    await LoadAsync();
            //    return Page();
            //}
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            user.ArName = Input.ArName;
            user.EnName = Input.EnName;
            user.IsNewsletter = Input.IsNewsletter;
            user.SecondEmail = Input.SecondEmail;
            user.NationalityId = Input.NationalityId;
            user.CountryId = Input.CountryId;
            user.CityId = Input.CityId;
            user.DateofBirth = Input.DateofBirth;
            user.Gender = Input.Gender;
            user.JobId = Input.JobName;
            user.UILanguage = Input.UILanguage;
            int[] startInfo = birthdate.Split('/').Select(s => int.Parse(s)).ToArray();
            user.DateofBirth = new DateTime(startInfo[2], startInfo[1], startInfo[0]);
            //user.DateofBirth = new DateTime(startInfo[0], startInfo[1], startInfo[2]);
            await _userManager.UpdateAsync(user);
            _context.SaveChanges();
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "تم تحديث بياناتك الشخصية بنجاح";
            return RedirectToPage();
        }
    }
}
