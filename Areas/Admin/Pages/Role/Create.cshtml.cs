using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using razorweb.models;

namespace razorweb.Areas.Admin.Pages.Role;
public class CreateModel : RoleManagerBase
{
   public CreateModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager,
      myBlogContext)
   {
   }
   [BindProperty]
   public InputModel Input { get; set; }
   public void OnGet()
   {
   }
   public async Task<IActionResult> OnPostAsync()
   {
      if( !ModelState.IsValid ) return Page();
      var newRole = new IdentityRole(Input.Name);
      var result = await _roleManager.CreateAsync(newRole);
      if( !result.Succeeded )
      {
         foreach(var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
         return Page();
      }
      StatusMessage = $"Create Success {Input.Name}";
      return RedirectToPage("Index");
   }
   public class InputModel
   {
      [Required]
      [Display(Name = "Role Name")]
      [StringLength(10, MinimumLength = 3, ErrorMessage = "{0} from {2} to {1}")]
      public string Name { get; set; }
   }
}