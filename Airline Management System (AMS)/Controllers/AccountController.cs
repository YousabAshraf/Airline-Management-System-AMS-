using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Airline_Management_System__AMS_.Models;
using Airline_Management_System__AMS_.ViewModels;
using Airline_Management_System__AMS_.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailSender _emailSender;
    private readonly ApplicationDbContext _context;

    public AccountController(UserManager<ApplicationUser> userManager,
                             SignInManager<ApplicationUser> signInManager,
                             RoleManager<IdentityRole> roleManager,
                             IEmailSender emailSender,
                             ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _emailSender = emailSender;
        _context = context;
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
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please correct the errors in the form.";
            return View(model);
        }
        var existingPassport = await _context.Users
          .AnyAsync(u => u.PassportNumber == model.PassportNumber);

        if (existingPassport)
        {
            ModelState.AddModelError("PassportNumber", "This passport number is already in use.");
            return View(model);
        }

        var code = new Random().Next(100000, 999999).ToString();

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            EmailConfirmed = false,
            EmailConfirmationCode = code,
            Role = model.Role,
            NationalId = model.NationalId,
            PassportNumber = model.PassportNumber,
            PhoneNumber = model.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }


        if (!await _roleManager.RoleExistsAsync(model.Role))
            await _roleManager.CreateAsync(new IdentityRole(model.Role));

        await _userManager.AddToRoleAsync(user, model.Role);

        user.LastVerificationEmailSent = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        await _emailSender.SendEmailAsync(
            user.Email,
            "Email Verification Code",
            $@"
    <div style='font-family: Arial, sans-serif; background-color:#f5f7fa; padding:30px;'>
        <div style='max-width:600px; margin:auto; background:white; padding:25px; border-radius:12px; box-shadow:0 2px 10px rgba(0,0,0,0.08);'>

            <!-- Banner -->
            <div style='text-align:center; margin-bottom:20px;'>
<img src='https://github.com/Steven-Amin02/Airline-Management-System-AMS-/raw/master/Airline%20Management%20System%20(AMS)/wwwroot/images/logo_in_email/readme_banner.png'
     alt='Banner'
     style='width:100%; border-radius:10px;' />
            </div>

            <!-- Title -->
            <h2 style='text-align:center; color:#333; margin-bottom:10px;'>
                Email Verification
            </h2>

            <p style='color:#555; font-size:15px; text-align:center;'>
                Welcome to <strong>Airline Management System</strong>!  
                Please use the verification code below to complete your registration.
            </p>

            <!-- Code Box -->
            <div style='margin:30px auto; text-align:center;'>
                <div style='display:inline-block; padding:15px 25px; border-radius:10px;
                            background:#004aad; color:white; font-size:24px; 
                            letter-spacing:5px; font-weight:bold;'>
                    {code}
                </div>
            </div>

            <p style='color:#555; font-size:14px; text-align:center;'>
                This code is valid for the next <strong>10 minutes</strong>.
            </p>

            <hr style='margin:30px 0; border:none; border-top:1px solid #ddd;'>
            <p style='text-align:center; font-size:13px; color:#888;'>
                If you didn't request this verification, you can safely ignore this email.
            </p>
        </div>
    </div>
    ");

        TempData["Success"] = "Verification code sent to your email.";
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
            user.EmailConfirmationCode = "CONFIRMED";
            user.VerificationResendCount = 0;
            await _userManager.UpdateAsync(user);

            // AUTO-CREATE PASSENGER PROFILE for Customer users
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("User") || roles.Contains("Customer"))
            {
                // Check if passenger profile already exists
                var existingPassenger = await _context.Passengers
                    .FirstOrDefaultAsync(p => p.UserId == user.Id);

                if (existingPassenger == null)
                {
                    var passenger = new Passenger
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        PassportNumber = user.PassportNumber,
                        NationalId = user.NationalId,
                        IsArchived = false
                    };

                    _context.Passengers.Add(passenger);
                    await _context.SaveChangesAsync();
                }
            }

            TempData["Success"] = "Account verified you can login now.";
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
            (DateTime.UtcNow - user.LastVerificationEmailSent.Value).TotalMinutes < 5 * user.VerificationResendCount)
        {
            var remaining = 5 * user.VerificationResendCount - (DateTime.UtcNow - user.LastVerificationEmailSent.Value).TotalMinutes;
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
        $@"
    <div style='font-family: Arial, sans-serif; background-color:#f5f7fa; padding:30px;'>
        <div style='max-width:600px; margin:auto; background:white; padding:25px; border-radius:12px; box-shadow:0 2px 10px rgba(0,0,0,0.08);'>

            <!-- Banner -->
            <div style='text-align:center; margin-bottom:20px;'>
<img src='https://github.com/Steven-Amin02/Airline-Management-System-AMS-/raw/master/Airline%20Management%20System%20(AMS)/wwwroot/images/logo_in_email/readme_banner.png'
     alt='Banner'
     style='width:100%; border-radius:10px;' />
            </div>

            <!-- Title -->
            <h2 style='text-align:center; color:#333; margin-bottom:10px;'>
                New Verification Code
            </h2>

            <p style='color:#555; font-size:15px; text-align:center;'>
                You requested a new verification code.  
                Please use the updated code below to complete your email verification.
            </p>

            <!-- Code Box -->
            <div style='margin:30px auto; text-align:center;'>
                <div style='display:inline-block; padding:15px 25px; border-radius:10px;
                            background:#004aad; color:white; font-size:24px; 
                            letter-spacing:5px; font-weight:bold;'>
                    {code}
                </div>
            </div>

            <p style='color:#555; font-size:14px; text-align:center;'>
                This code is valid for the next <strong>10 minutes</strong>.
            </p>

            <hr style='margin:30px 0; border:none; border-top:1px solid #ddd;'>
            <p style='text-align:center; font-size:13px; color:#888;'>
                If you didn't request this email, please ignore it.
            </p>
        </div>
    </div>
    ");



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

        ModelState.AddModelError("", "Wrong Password, Please try again.");
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

        await _emailSender.SendEmailAsync(
     user.Email,
     "Email Verification Code",
     $@"
    <div style='font-family: Arial, sans-serif; background-color:#f5f7fa; padding:30px;'>
        <div style='max-width:600px; margin:auto; background:white; padding:25px; border-radius:12px; box-shadow:0 2px 10px rgba(0,0,0,0.08);'>

            <!-- Banner -->
            <div style='text-align:center; margin-bottom:20px;'>
<img src='https://github.com/Steven-Amin02/Airline-Management-System-AMS-/raw/master/Airline%20Management%20System%20(AMS)/wwwroot/images/logo_in_email/readme_banner.png'
     alt='Banner'
     style='width:100%; border-radius:10px;' />
            </div>

            <!-- Title -->
            <h2 style='text-align:center; color:#333; margin-bottom:10px;'>
                Email Verification
            </h2>

            <p style='color:#555; font-size:15px; text-align:center;'>
                Welcome to <strong>Airline Management System</strong>!  
                Please use the verification code below to complete your registration.
            </p>

            <!-- Code Box -->
            <div style='margin:30px auto; text-align:center;'>
                <div style='display:inline-block; padding:15px 25px; border-radius:10px;
                            background:#004aad; color:white; font-size:24px; 
                            letter-spacing:5px; font-weight:bold;'>
                    {code}
                </div>
            </div>

            <p style='color:#555; font-size:14px; text-align:center;'>
                This code is valid for the next <strong>10 minutes</strong>.
            </p>

            <hr style='margin:30px 0; border:none; border-top:1px solid #ddd;'>
            <p style='text-align:center; font-size:13px; color:#888;'>
                If you didn't request this verification, you can safely ignore this email.
            </p>
        </div>
    </div>
    ");



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


    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login");
        }

        var passenger = await _context.Passengers.FirstOrDefaultAsync(p => p.UserId == user.Id);
        if (passenger == null)
        {
            // Fallback: If no passenger profile exists (e.g. legacy admin), create one or show error
            // For now, we'll redirect to home or show a message
            return RedirectToAction("Index", "Home");
        }

        return View(passenger);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(Passenger model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login");
        }

        // Ensure the user is editing their own profile
        if (model.UserId != user.Id)
        {
            return Unauthorized();
        }

        // Remove validation for fields the user can't change or hidden fields if necessary
        ModelState.Remove("User");
        ModelState.Remove("UserId");

        if (ModelState.IsValid)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var passenger = await _context.Passengers.FirstOrDefaultAsync(p => p.Id == model.Id);
                if (passenger == null)
                {
                    return NotFound();
                }

                // 1. Update Passenger Record
                passenger.FirstName = model.FirstName;
                passenger.LastName = model.LastName;
                passenger.PhoneNumber = model.PhoneNumber;
                passenger.PassportNumber = model.PassportNumber;
                passenger.NationalId = model.NationalId;

                // Only update Email if it's different (might require re-verification logic in a real app, strict for now)
                bool emailChanged = !string.Equals(passenger.Email, model.Email, StringComparison.OrdinalIgnoreCase);
                passenger.Email = model.Email;

                _context.Passengers.Update(passenger);

                // 2. Sync with ApplicationUser
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.PassportNumber = model.PassportNumber; // Extended property
                user.NationalId = model.NationalId;       // Extended property

                if (emailChanged)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email; // Keep username synced with email
                    user.EmailConfirmed = true; // Force re-verification? Or trust user? Let's keep it simple for now and trust
                    // If you want to force verification, you'd generate a token here.
                }

                var userUpdateResult = await _userManager.UpdateAsync(user);
                if (!userUpdateResult.Succeeded)
                {
                    throw new Exception("Failed to update user account: " + string.Join(", ", userUpdateResult.Errors.Select(e => e.Description)));
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // 3. Refresh Sign-in Principal (so the new name/claims appear immediately)
                await _signInManager.RefreshSignInAsync(user);

                TempData["Success"] = "Profile updated successfully!";
                return RedirectToAction("Index", "UserDashboard"); // Redirect to Dashboard
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "Error updating profile: " + ex.Message);
            }
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
