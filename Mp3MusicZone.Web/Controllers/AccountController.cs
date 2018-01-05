namespace Mp3MusicZone.Web.Controllers
{
	using AutoMapper;
	using Data.Models;
	using Infrastructure.Extensions;
	using Infrastructure.Filters;
	using Microsoft.AspNetCore.Authentication;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;
	using Models.Account;
	using Services.Contracts;
	using System;
	using System.Security.Claims;
	using System.Threading.Tasks;

	[Authorize]
	[Route("[controller]/[action]")]
	public class AccountController : Controller
	{
		private readonly UserManager<User> userManager;
		private readonly SignInManager<User> signInManager;
		private readonly IEmailSenderService emailSender;
		private readonly ILogger logger;

		public AccountController(
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			IEmailSenderService emailSender,
			ILogger<AccountController> logger)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.emailSender = emailSender;
			this.logger = logger;
		}

		[TempData]
		public string ErrorMessage { get; set; }

		[AllowAnonymous]
		public async Task<IActionResult> Login(string returnUrl = null)
		{
			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateModelState]
		public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;

			// Require the user to have a confirmed email before they can log on.
			User user = await userManager.FindByNameAsync(model.Username);

			if (user != null)
			{
				if (!await userManager.IsEmailConfirmedAsync(user))
				{
					TempData.AddErrorMessage("You must have a confirmed e-mail to log in.");
					//ModelState.AddModelError(
					//	string.Empty,
					//	"You must have a confirmed email to log in.");

					return View(model);
				}
			}

			// This doesn't count login failures towards account lockout
			// To enable password failures to trigger account lockout, set lockoutOnFailure: true
			var result = await signInManager.PasswordSignInAsync(
				model.Username,
				model.Password,
				model.RememberMe,
				lockoutOnFailure: false);

			if (result.Succeeded)
			{
				logger.LogInformation("User logged in.");
				return this.RedirectToLocal(returnUrl);
			}

			//if (result.RequiresTwoFactor)
			//{
			//    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
			//}

			if (result.IsLockedOut)
			{
				logger.LogWarning("User account locked out.");

				return RedirectToAction(nameof(Lockout));
			}

			// If we got this far, something failed, redisplay form
			TempData.AddErrorMessage("Invalid login attempt.");
			return View(model);
		}

		[AllowAnonymous]
		public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
		{
			// Ensure the user has gone through the username & password screen first
			var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

			if (user == null)
			{
				throw new ApplicationException($"Unable to load two-factor authentication user.");
			}

			var model = new LoginWith2faViewModel { RememberMe = rememberMe };
			ViewData["ReturnUrl"] = returnUrl;

			return View(model);
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateModelState]
		public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
		{
			var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
			}

			var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

			var result = await signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

			if (result.Succeeded)
			{
				logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
				return this.RedirectToLocal(returnUrl);
			}
			else if (result.IsLockedOut)
			{
				logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
				return RedirectToAction(nameof(Lockout));
			}
			else
			{
				logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
				ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
				return View();
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
		{
			// Ensure the user has gone through the username & password screen first
			var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
			if (user == null)
			{
				throw new ApplicationException($"Unable to load two-factor authentication user.");
			}

			ViewData["ReturnUrl"] = returnUrl;

			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateModelState]
		public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
		{
			var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
			if (user == null)
			{
				throw new ApplicationException($"Unable to load two-factor authentication user.");
			}

			var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

			var result = await signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

			if (result.Succeeded)
			{
				logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
				return this.RedirectToLocal(returnUrl);
			}
			if (result.IsLockedOut)
			{
				logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
				return RedirectToAction(nameof(Lockout));
			}
			else
			{
				logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
				ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
				return View();
			}
		}

		[AllowAnonymous]
		public IActionResult Lockout()
		{
			return View();
		}

		[AllowAnonymous]
		public IActionResult Register(string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateModelState]
		public async Task<IActionResult> Register(RegisterViewModel model,
			string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			User user = await userManager.FindByEmailAsync(model.Email);

			if (user != null)
			{
				this.TempData.AddErrorMessage($"E-mail address '{model.Email}' is already taken.");
				return View(model);
			}

			user = new User
			{
				UserName = model.Username,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Email = model.Email,
				Birthdate = model.Birthdate,
				Genre = model.Genre
			};

			IdentityResult result = await userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				logger.LogInformation("User created a new account with password.");

				string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
				string callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);

				await emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

				//await signInManager.SignInAsync(user, isPersistent: false);
				logger.LogInformation("User created a new account with password.");

				if (returnUrl is null)
				{
					TempData.AddSuccessMessage($"The registration is successfull. Please, verify your e-mail address {model.Email} before proceeding.");

					return View(model);
				}

				return this.RedirectToLocal(returnUrl);
			}
			
			// If we got this far, something failed, redisplay form
			this.AddErrors(result);
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			logger.LogInformation("User logged out.");
			return RedirectToAction(nameof(HomeController.Index), "Home");
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult ExternalLogin(string provider, string returnUrl = null)
		{
			// Request a redirect to the external login provider.
			var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
			var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
			return Challenge(properties, provider);
		}

		[AllowAnonymous]
		public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
		{
			if (remoteError != null)
			{
				ErrorMessage = $"Error from external provider: {remoteError}";
				return RedirectToAction(nameof(Login));
			}
			var info = await signInManager.GetExternalLoginInfoAsync();
			if (info == null)
			{
				return RedirectToAction(nameof(Login));
			}

			// Sign in the user with this external login provider if the user already has a login.
			var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
			if (result.Succeeded)
			{
				logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
				return this.RedirectToLocal(returnUrl);
			}
			if (result.IsLockedOut)
			{
				return RedirectToAction(nameof(Lockout));
			}
			else
			{
				// If the user does not have an account, then ask the user to create an account.
				ViewData["ReturnUrl"] = returnUrl;
				ViewData["LoginProvider"] = info.LoginProvider;
				var email = info.Principal.FindFirstValue(ClaimTypes.Email);
				return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
			}
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
		{
			if (ModelState.IsValid)
			{
				// Get the information about the user from the external login provider
				var info = await signInManager.GetExternalLoginInfoAsync();
				if (info == null)
				{
					throw new ApplicationException("Error loading external login information during confirmation.");
				}
				var user = new User { UserName = model.Email, Email = model.Email };
				var result = await userManager.CreateAsync(user);
				if (result.Succeeded)
				{
					result = await userManager.AddLoginAsync(user, info);
					if (result.Succeeded)
					{
						await signInManager.SignInAsync(user, isPersistent: false);
						logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
						return this.RedirectToLocal(returnUrl);
					}
				}

				this.AddErrors(result);
			}

			ViewData["ReturnUrl"] = returnUrl;
			return View(nameof(ExternalLogin), model);
		}

		[AllowAnonymous]
		public async Task<IActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return RedirectToAction(nameof(HomeController.Index), "Home");
			}
			var user = await userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{userId}'.");
			}
			var result = await userManager.ConfirmEmailAsync(user, code);
			return View(result.Succeeded ? "ConfirmEmail" : "Error");
		}

		[AllowAnonymous]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByEmailAsync(model.Email);
				if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
				{
					// Don't reveal that the user does not exist or is not confirmed
					return RedirectToAction(nameof(ForgotPasswordConfirmation));
				}

				// For more information on how to enable account confirmation and password reset please
				// visit https://go.microsoft.com/fwlink/?LinkID=532713
				var code = await userManager.GeneratePasswordResetTokenAsync(user);
				var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
				await emailSender.SendEmailAsync(model.Email, "Reset Password",
				   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
				return RedirectToAction(nameof(ForgotPasswordConfirmation));
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		[AllowAnonymous]
		public IActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		[AllowAnonymous]
		public IActionResult ResetPassword(string code = null)
		{
			if (code == null)
			{
				throw new ApplicationException("A code must be supplied for password reset.");
			}
			var model = new ResetPasswordViewModel { Code = code };
			return View(model);
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await userManager.FindByEmailAsync(model.Email);

			if (user == null)
			{
				// Don't reveal that the user does not exist
				return RedirectToAction(nameof(ResetPasswordConfirmation));
			}

			var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);

			if (result.Succeeded)
			{
				return RedirectToAction(nameof(ResetPasswordConfirmation));
			}

			this.AddErrors(result);

			return View();
		}

		[AllowAnonymous]
		public IActionResult ResetPasswordConfirmation()
		{
			return View();
		}

		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
