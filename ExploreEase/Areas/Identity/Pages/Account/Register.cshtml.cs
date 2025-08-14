#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Models.Models;
using Services.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace ExploreEase.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ExploreEaseUser> _signInManager;
        private readonly UserManager<ExploreEaseUser> _userManager;
        private readonly IUserStore<ExploreEaseUser> _userStore;
        private readonly IUserEmailStore<ExploreEaseUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly EmailServices _emailServices;

        public RegisterModel(
            UserManager<ExploreEaseUser> userManager,
            IUserStore<ExploreEaseUser> userStore,
            SignInManager<ExploreEaseUser> signInManager,
            ILogger<RegisterModel> logger, EmailServices emailServices)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailServices = emailServices;

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Display(Name = "Role")]
            public string Role { get; set; }

            [Required]
            [StringLength(15, MinimumLength = 3, ErrorMessage = "Character length must be 3 to 15 letters long")]
            [Display(Name = "Full Name")]
            [UsernameNotAdmin(ErrorMessage = "The Username is not available")]
            public string FullName { get; set; }

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
        }

        public class UsernameNotAdminAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null && value.ToString().ToLower() == "admin12")
                {
                    return new ValidationResult(ErrorMessage);
                }
                return ValidationResult.Success;
            }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // Check if email domain has MX record
                if (!await _emailServices.DomainHasMxRecordAsync(Input.Email))
                {
                    ModelState.AddModelError("Input.Email", "Email domain is invalid or does not exist.");
                    return Page();
                }
                // Check if email already exists
                var existingUser = await _userManager.FindByEmailAsync(Input.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Input.Email", "Email is already registered.");
                    return Page();
                }

                // Create user
                var user = CreateUser();
                user.FullName = Input.FullName;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, "User");

                    // Generate email confirmation token
                    var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _userManager.ConfirmEmailAsync(user, emailConfirmationToken);

                    // Send success email
                    var subject = "Welcome to ExploreEase - Account Created Successfully";
                    var body = $"<p>Hi {Input.FullName},</p><p>Your account has been successfully created.</p>";
                    var emailSent = _emailServices.SendEmail(Input.Email, subject, body);

                    if (!emailSent)
                    {
                        _logger.LogWarning($"Failed to send registration email to {Input.Email}");
                        // Optionally add a message or retry mechanism
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    TempData["SuccessMessage"] = "Account successfully created!";
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            return Page();
        }

        private ExploreEaseUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ExploreEaseUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ExploreEaseUser)}'. " +
                    $"Ensure that '{nameof(ExploreEaseUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ExploreEaseUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ExploreEaseUser>)_userStore;
        }
    }
}
