using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorweb.models;

namespace razorweb.Areas.Admin.Pages.Role;
public class RoleManagerBase : PageModel
{
   protected readonly MyBlogContext _myBlogContext;
   protected readonly RoleManager<IdentityRole> _roleManager;
   public RoleManagerBase(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext)
   {
      _roleManager   = roleManager;
      _myBlogContext = myBlogContext;
   }
   [TempData]
   public string StatusMessage { get; set; }
}