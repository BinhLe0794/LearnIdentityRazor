using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace razorweb.Areas.Identity.Pages.Account;
[AllowAnonymous]
public class ForgotPasswordConfirmation : PageModel
{
   public void OnGet()
   {
   }
}