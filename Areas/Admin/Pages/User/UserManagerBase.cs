using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorweb.models;

namespace razorweb.Areas.Admin.Pages.User;
public class UserManagerBase : PageModel
{
   protected readonly MyBlogContext _myBlogContext;
   protected readonly UserManager<AppUser> _userManager;
   public UserManagerBase(UserManager<AppUser> userManager, MyBlogContext myBlogContext)
   {
      _userManager   = userManager;
      _myBlogContext = myBlogContext;
   }
   [TempData]
   public string StatusMessage { get; set; }
}