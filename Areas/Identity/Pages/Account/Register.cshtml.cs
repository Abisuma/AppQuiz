// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using AppQuiz.Data;
using AppQuiz.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppQuiz.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;//adding a role manager
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
        ILogger<RegisterModel> logger,
            IEmailSender emailSender,IWebHostEnvironment webHostEnvironment, AppDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _roleManager = roleManager; 
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Role { get; set; }
            public IEnumerable<SelectListItem> RoleLists { get; set; }
            public int SelectedDepartmentId { get; set; }
           
            public string SelectedDepartmentName { get; set; }
            public IEnumerable<SelectListItem> DepartmentList { get; set; }
            [Required]
            public string NameOfUser { get; set; }
            
            public IFormFile ProfileImage { get; set; }
        
        }



        public async Task OnGetAsync(string returnUrl = null)
        {
            try
            {
                if (!_roleManager.RoleExistsAsync(SD.Role_Users).GetAwaiter().GetResult())
                {
                    
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Users)).GetAwaiter().GetResult();//creating users
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();

                }
                
            }

            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
            }


            var departments = _context.Departments.ToList();

            // Create a SelectList for the dropdown

            var departmentList = new SelectList(departments, "DepartmentId", "DepartmentalName");

            Input = new InputModel
            {
                
                RoleLists = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }),

                DepartmentList = departmentList
            };
            



            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ////using projection to populate a drop down list
           
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                user.NameOfUser = Input.NameOfUser;
                string selectedRole = Input.Role;

                

                if (selectedRole == "Admin")
                {
                    // Set the default department name for Admin
                    Input.SelectedDepartmentName = "AdminDepartment";
                }

                if (Input.Role == "Admin")
                {
                    var department = _context.Departments.FirstOrDefault(d => d.DepartmentalName == Input.SelectedDepartmentName);
                    // Set the department based on the selected department name
                    user.Department = department;
                }
                else
                {
                    // Set the department based on the selected department ID
                    user.DepartmentId = Input.SelectedDepartmentId;
                }
                //user.DepartmentId = Input.SelectedDepartmentId;
                

                if (Input.ProfileImage != null)
                {
                    // Save the image to a specific directory
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"Images\ProfileImages");
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(Input.ProfileImage.FileName);
                    var imagePathWithFileName = Path.Combine(imagePath, imageName);

                    using (var stream = new FileStream(imagePathWithFileName, FileMode.Create))
                    {
                        await Input.ProfileImage.CopyToAsync(stream);
                    }

                    user.ImageUrl = @"\Images\ProfileImages" + imageName; // Save the URL to the image
                }
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (!String.IsNullOrEmpty(Input.Role))
                    {
                        await _userManager.AddToRoleAsync(user, Input.Role);//add the specified user a role

                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Users);
                    }

                    var departmentId = Input.SelectedDepartmentId;

                    if (departmentId != null)
                    {
                        
                        TempData["SelectedDepartmentId"] = departmentId; // Store in TempData
                    }

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

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

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
