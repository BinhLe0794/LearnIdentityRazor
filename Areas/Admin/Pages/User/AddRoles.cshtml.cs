using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using razorweb.models;

namespace razorweb.Areas.Admin.Pages.User;
public class AddRolesModel : UserManagerBase
{
   private readonly RoleManager<IdentityRole> _roleManager;
   public AddRolesModel(
      UserManager<AppUser> userManager,
      MyBlogContext myBlogContext,
      RoleManager<IdentityRole> roleManager)
      : base(userManager, myBlogContext)
   {
      _roleManager = roleManager;
   }
   [BindProperty]
   public new AppUser User { get; set; }
   [BindProperty]
   public string[] RoleNames { get; set; }
   public SelectList SelectListRoles { get; set; }
   public async Task<IActionResult> OnGet(string userId)
   {
      if( string.IsNullOrEmpty(userId) ) return NotFound("Cant find User");
      User = await _userManager.FindByIdAsync(userId);
      if( User == null ) return NotFound("Cant find User");
      RoleNames = (await _userManager.GetRolesAsync(User)).ToArray();
      var roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
      SelectListRoles = new SelectList(roleNames);
      return Page();
   }
   public async Task<IActionResult> OnPost(string userId)
   {
      if( string.IsNullOrEmpty(userId) ) return NotFound("Cant find User");
      User = await _userManager.FindByIdAsync(userId);
      if( User == null ) return NotFound("Cant find User");
      var oldRoleNames = (await _userManager.GetRolesAsync(User)).ToArray();
      var deleteRoles = oldRoleNames.Where(r => !RoleNames.Contains(r));
      var addRoles = RoleNames.Where(r => !oldRoleNames.Contains(r));
      var deleteResult = await _userManager.RemoveFromRolesAsync(User, deleteRoles);
      if( !deleteResult.Succeeded )
      {
         foreach(var error in deleteResult.Errors) ModelState.AddModelError(string.Empty, error.Description);
         RoleNames       = (await _userManager.GetRolesAsync(User)).ToArray();
         SelectListRoles = new SelectList(await _roleManager.Roles.Select(r => r.Name).ToListAsync());
         return Page();
      }
      var addResult = await _userManager.AddToRolesAsync(User, addRoles);
      if( !addResult.Succeeded )
      {
         foreach(var error in addResult.Errors) ModelState.AddModelError(string.Empty, error.Description);
         RoleNames       = (await _userManager.GetRolesAsync(User)).ToArray();
         SelectListRoles = new SelectList(await _roleManager.Roles.Select(r => r.Name).ToListAsync());
         return Page();
      }
      StatusMessage = $"Update {RoleNames.Count()}";
      return RedirectToPage("Index");
   }
}