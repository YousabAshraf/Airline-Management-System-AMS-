using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Airline_Management_System__AMS_.Models;
using Airline_Management_System__AMS_.ViewModels;
using System;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailSender _emailSender;

    public AccountController(UserManager<ApplicationUser> userManager,
                             SignInManager<ApplicationUser> signInManager,
                             RoleManager<IdentityRole> roleManager,
                             IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _emailSender = emailSender;
    }

    // Register GET
    [HttpGet]
    public IActionResult Register()
    {
        var roles = new List<string> { "User", "Admin" };
        ViewBag.Roles = roles;
        return View();
    }


    // Register POST
    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var code = new Random().Next(100000, 999999).ToString();

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            EmailConfirmed = false,
            EmailConfirmationCode = code,
            Role = model.Role
        };

        // 1️⃣ إنشاء المستخدم أولاً
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }

        // 2️⃣ إضافة الدور
        if (!await _roleManager.RoleExistsAsync(model.Role))
            await _roleManager.CreateAsync(new IdentityRole(model.Role));

        await _userManager.AddToRoleAsync(user, model.Role);

        // 3️⃣ حفظ وقت إرسال الكود بعد نجاح الـ Create
        user.LastVerificationEmailSent = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        // 4️⃣ إرسال الإيميل
        await _emailSender.SendEmailAsync(user.Email, "Email Verification Code",
            $"Your verification code is: <b>{code}</b>");

        return RedirectToAction("VerifyEmail", new { userId = user.Id });
    }


    // VerifyEmail GET
    [HttpGet]
    public IActionResult VerifyEmail(string userId)
    {
        return View(new VerifyEmailViewModel { UserId = userId });
    }



    // VerifyEmail POST
    [HttpPost]
    public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null) return NotFound();

        if (user.EmailConfirmationCode == model.Code)
        {
            user.EmailConfirmed = true;
            user.EmailConfirmationCode = "CONFIRMED"; // بعد التفعيل نحذف الكود
            user.VerificationResendCount = 0;
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Login");
        }

        TempData["Error"] = "The verification code is incorrect. Please try again.";

        return RedirectToAction("VerifyEmail", new { userId = model.UserId });
    }


    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> ResendCode(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        if (user.VerificationResendCount >= 5)
        {
            TempData["Error"] = "You have reached the maximum number of resend attempts.";
            return RedirectToAction("VerifyEmail", new { userId });
        }

        if (user.LastVerificationEmailSent != null &&
            (DateTime.UtcNow - user.LastVerificationEmailSent.Value).TotalMinutes < 5* user.VerificationResendCount)
        {
            var remaining = 5* user.VerificationResendCount - (DateTime.UtcNow - user.LastVerificationEmailSent.Value).TotalMinutes;
            TempData["Error"] = $"Please wait {Math.Ceiling(remaining)} minutes before requesting another code.";
            return RedirectToAction("VerifyEmail", new { userId });
        }

        var code = new Random().Next(100000, 999999).ToString();
        user.EmailConfirmationCode = code;

        user.LastVerificationEmailSent = DateTime.UtcNow;

        user.VerificationResendCount++;

        await _userManager.UpdateAsync(user);

        await _emailSender.SendEmailAsync(
            user.Email,
            "New Verification Code",
            $"Your new verification code is: <b>{code}</b>"
        );

        TempData["Success"] = "A new verification code has been sent to your email.";

        return RedirectToAction("VerifyEmail", new { userId });
    }
    [HttpPost]
    public async Task<IActionResult> CancelVerification(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }

        return RedirectToAction("Register");
    }


    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            ModelState.AddModelError("", "No account found with this email.");
            return View(model);
        }

        if (!user.EmailConfirmed)
        {
            TempData["Error"] = "You need to confirm your email first.";
            return RedirectToAction("VerifyEmail", new { userId = user.Id });
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

        if (result.Succeeded)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
                return RedirectToAction("Index", "AdminDashboard");
            else if (roles.Contains("User"))
                return RedirectToAction("Index", "UserDashboard");
            else
                return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid login attempt.");
        return View(model);
    }
    [HttpGet]
    public IActionResult ForgetPassword() => View();

    [HttpPost]
    public async Task<IActionResult> ForgetPassword(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            ModelState.AddModelError("", "Please enter your email.");
            return View();
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            ModelState.AddModelError("", "No account found with this email.");
            return View();
        }

        var code = new Random().Next(100000, 999999).ToString();
        user.EmailConfirmationCode = code;
        user.LastVerificationEmailSent = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        await _emailSender.SendEmailAsync(user.Email, "Password Reset Code",
            $"Your password reset verification code is: <b>{code}</b>");

        
        TempData["Success"] = "Verification code sent to your email.";
        return RedirectToAction("ResetPassword", new { userId = user.Id });
    }
    [HttpGet]
    public IActionResult ResetPassword(string userId)
    {
        return View(new ResetPasswordViewModel { UserId = userId });
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null) return NotFound();

        if (user.EmailConfirmationCode != model.VerificationCode)
        {
            ModelState.AddModelError("", "Invalid verification code.");
            return View(model);
        }

        if (model.NewPassword != model.ConfirmPassword)
        {
            ModelState.AddModelError("", "Passwords do not match.");
            return View(model);
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }

        user.EmailConfirmationCode = "CONFIRMED";
        await _userManager.UpdateAsync(user);

        TempData["Success"] = "Password has been reset successfully. You can now log in.";
        return RedirectToAction("Login");
    }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
