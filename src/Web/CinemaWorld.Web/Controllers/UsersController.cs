namespace CinemaWorld.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using CinemaWorld.Common;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Web.Areas.Identity.Pages.Account.InputModels;
    using CinemaWorld.Web.Areas.Identity.Pages.Account.Manage.InputModels;
    using CinemaWorld.Web.Infrastructure;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;

    public class UsersController : Controller
    {
        private readonly SignInManager<CinemaWorldUser> signInManager;
        private readonly UserManager<CinemaWorldUser> userManager;
        private readonly ILogger<UsersController> logger;
        private readonly IEmailSender emailSender;

        public UsersController(
            SignInManager<CinemaWorldUser> signInManager,
            UserManager<CinemaWorldUser> userManager,
            ILogger<UsersController> logger,
            IEmailSender emailSender)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.logger = logger;
            this.emailSender = emailSender;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        public string ReturnUrl { get; set; }

        public string LoginProvider { get; set; }

        [HttpPost]
        public async Task<IActionResult> AjaxLogin(AjaxLoginInputModel loginInput, string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            var ajaxObject = new AjaxObject();
            if (this.ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await this.signInManager
                    .PasswordSignInAsync(loginInput.Username, loginInput.Password, loginInput.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    this.logger.LogInformation("User logged in.");
                    ajaxObject.Success = true;
                    ajaxObject.Message = "Logged-in";
                    ajaxObject.Action = returnUrl;
                }

                if (result.IsLockedOut)
                {
                    this.logger.LogWarning("This account has been locked out, please try again later.");
                    return this.RedirectToPage("./Lockout");
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "The username or password supplied are incorrect. Please check your spelling and try again.");
                }
            }

            // login was unsuccessful, return model errors
            if (!ajaxObject.Success)
            {
                ajaxObject.Message = this.ModelErorrs(this.ModelState);
            }

            var jsonResult = new JsonResult(ajaxObject);
            return jsonResult;
        }

        [HttpPost]
        public async Task<IActionResult> AjaxRegister(AjaxRegisterInputModel registerInput, string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            var ajaxObject = new AjaxObject();
            if (this.ModelState.IsValid)
            {
                Enum.TryParse<Gender>(registerInput.SelectedGender, out Gender gender);

                var user = new CinemaWorldUser
                {
                    UserName = registerInput.Username,
                    Email = registerInput.Email,
                    FullName = registerInput.FullName,
                    Gender = gender,
                };

                var result = await this.userManager.CreateAsync(user, registerInput.Password);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");
                    await this.userManager.AddToRoleAsync(user, GlobalConstants.UserRoleName);

                    ajaxObject.Success = true;
                    ajaxObject.Message = "Registered-in";
                    ajaxObject.Action = returnUrl;

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = this.Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: this.Request.Scheme);

                    await this.emailSender.SendEmailAsync(
                        registerInput.Email,
                        "Confirm your email",
                        htmlMessage: $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = registerInput.Email });
                    }
                    else
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            if (!ajaxObject.Success)
            {
                ajaxObject.Message = this.ModelErorrs(this.ModelState);
            }

            var jsonResult = new JsonResult(ajaxObject);
            return jsonResult;
        }

        public IActionResult FacebookLogin()
        {
            var properties = this.signInManager
                .ConfigureExternalAuthenticationProperties("Facebook", this.Url.Action("ExternalLoginCallback", "Users"));

            return this.Challenge(properties, "Facebook");
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= this.Url.Content("~/");

            if (remoteError != null)
            {
                this.ErrorMessage = $"Error from external provider: {remoteError}";
                return this.RedirectToPage("/Login", new { ReturnUrl = returnUrl });
            }

            var info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                this.ErrorMessage = "Error loading external login information.";
                return this.RedirectToPage("/Login", new { ReturnUrl = returnUrl });
            }

            var result = await this.signInManager
                .ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                this.logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return this.LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return this.RedirectToPage("/Lockout");
            }
            else
            {
                this.ReturnUrl = returnUrl;
                this.LoginProvider = info.LoginProvider;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    var input = new ExternalLoginInputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                        FullName = info.Principal.FindFirstValue(ClaimTypes.Name),
                    };
                }

                return this.Redirect("~/");
            }

            // TODO
            //to associate a local user account to an external login provider
            //await _userInManager.AddLoginAsync(aUserYoullHaveToCreate, info);
        }

        private string ModelErorrs(ModelStateDictionary modelState)
        {
            return string.Join(Environment.NewLine, modelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
        }
    }
}
