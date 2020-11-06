using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CafeBites.Customs;
using CafeBites.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace CafeBites.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        //private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> roleManager;

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<RegisterModel> logger, /*IEmailSender emailSender,*/ RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;
            this.roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string Country { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Street { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            string role = Request.Form["userRoles"].ToString();
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new CafeBitesUser()
                {
                    UserName = Input.Email,
                    Name = Input.Name,
                    Email = Input.Email,
                    PhoneNumber = Input.PhoneNumber,
                    Country = Input.Country,
                    City = Input.City,
                    PostalCode = Input.PostalCode,
                    Street = Input.Street
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if(!await roleManager.RoleExistsAsync(StaticStrings.CounterExecutive))
                    {
                        await roleManager.CreateAsync(new IdentityRole(StaticStrings.CounterExecutive));
                    }
                    if (!await roleManager.RoleExistsAsync(StaticStrings.Customer))
                    {
                        await roleManager.CreateAsync(new IdentityRole(StaticStrings.Customer));
                    }
                    if (!await roleManager.RoleExistsAsync(StaticStrings.FoodManager))
                    {
                        await roleManager.CreateAsync(new IdentityRole(StaticStrings.FoodManager));
                    }
                    if (!await roleManager.RoleExistsAsync(StaticStrings.Manager))
                    {
                        await roleManager.CreateAsync(new IdentityRole(StaticStrings.Manager));
                    }
                    
                    if (role == StaticStrings.FoodManager)
                    {
                        await _userManager.AddToRoleAsync(user, StaticStrings.FoodManager);
                    }
                    else
                    {
                        if (role == StaticStrings.CounterExecutive)
                        {
                            await _userManager.AddToRoleAsync(user, StaticStrings.CounterExecutive);
                        }

                        if (role == StaticStrings.Manager)
                        {
                            await _userManager.AddToRoleAsync(user, StaticStrings.Manager);
                        }

                        else
                        {
                            await _userManager.AddToRoleAsync(user, StaticStrings.Customer);
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }

                    //return RedirectToAction("Index", "CafeBitesUser", new { area = "Admin" });

                    //_logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    //}
                    //else
                    //{
                    //    await _signInManager.SignInAsync(user, isPersistent: false);
                    //    return LocalRedirect(returnUrl);
                    //}
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
