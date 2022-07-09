using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using razorweb.models;

namespace razorweb.Areas.Identity.Pages.Account;
[AllowAnonymous]
public class LogoutModel : PageModel
{
   private readonly ILogger<LogoutModel> _logger;
   private readonly SignInManager<AppUser> _signInManager;
   public LogoutModel(SignInManager<AppUser> signInManager, ILogger<LogoutModel> logger)
   {
      _signInManager = signInManager;
      _logger        = logger;
   }
   public void OnGet()
   {
   }
   public async Task<IActionResult> OnPost(string returnUrl = null)
   {
      await _signInManager.SignOutAsync();
      _logger.LogInformation("User logged out.");
      _logger.LogWarning(returnUrl);
      if( returnUrl != null )
         return LocalRedirect(returnUrl);
      return RedirectToPage();
   }
}