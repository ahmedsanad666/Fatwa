using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using WebOS.Data;
using WebOS.Models;

namespace WebOS.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, ApplicationDbContext context, RoleManager<IdentityRole> roleMgr)
        {
            roleManager = roleMgr;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "حقل البريد الالكتروني ضروري")]
            [EmailAddress]
            [Display(Name = "البريد الإلكتروني")]
            public string Email { get; set; }

            [Required(ErrorMessage = "حقل الاسم بالعربي ضروري")]
            [StringLength(30,ErrorMessage ="لقد قمت بإدخال نص طويل")]
            [Display(Name = "الإسم بالعربي")]
            public string ArName { get; set; }

            [Required(ErrorMessage = "حقل كلمة المرور ضروري")]
            [DataType(DataType.Password)]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [Display(Name = "كلمة المرور")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تأكيد كلمة المرور")]
            [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (_context.Country.Count() == 0)
            {
                _context.Country.Add(new Country()
                {
                    ShortName = "other",
                    ArCountryName = "أُخرى",
                    CountryCode = "other",
                    EnCountryName = "other",
                    Flag = "other",
                });
                _context.SaveChanges();
            }


            if (_context.City.Count() == 0)
            {
                _context.City.Add(new City()
                {
                    ArCityName = "أُخرى",
                    CountryId = _context.Country.OrderBy(c => c.Id).First().Id,
                    EnCityName = "other",
                });
                _context.SaveChanges();
            }
            var userscounts = _context.ApplicationUsers.Count();
            
           
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, ArName=Input.ArName ,CityId = _context.City.OrderBy(c => c.Id).First().Id, CountryId = _context.Country.OrderBy(c => c.Id).First().Id, NationalityId= _context.Country.OrderBy(c => c.Id).First().Id };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (userscounts == 0)
                    {
                        IdentityResult roleresult
    = await roleManager.CreateAsync(new IdentityRole("Admins"));

                        if (roleresult.Succeeded)
                        {
                            IdentityResult userresult = await _userManager.AddToRoleAsync(user, "Admins");
                            if (!result.Succeeded)
                            {
                                AddErrorsFromResult(result);
                            }
                            _context.SaveChanges();

                        }

                        else
                        {
                            AddErrorsFromResult(roleresult);
                        }

                    }

                    

                    user.RegistrationDate = DateTime.Now;
                    user.SupportPin = Guid.NewGuid().ToString().Substring(0,4);
                    _context.ApplicationUsers.Update(user);
                    _context.SaveChanges();

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            void AddErrorsFromResult(IdentityResult result)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
