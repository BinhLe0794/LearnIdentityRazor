using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorweb.models;

namespace razorweb.Areas.Admin.Pages.Role;
public class EditRoleClaimModel : RoleManagerBase
{
   public EditRoleClaimModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(
      roleManager,
      myBlogContext)
   {
   }
   public class InputModel
   {
      public string Type { get; set; } = string.Empty;
      public string Value { get; set; } = string.Empty;
   }
   [BindProperty]
   public InputModel Input { get; set; } = new();
   public IdentityRole Role { get; set; } = new();
   public IdentityRoleClaim<string> Claim { get; set; } = new();

   public async Task<IActionResult> OnGet(int? claimId)
   {
      if( claimId == null )
      {
         return NotFound("Cant find Claims");
      }
      var claim = await _myBlogContext.RoleClaims.FirstOrDefaultAsync(x => x.Id == claimId);
      if( claim == null )
      {
         return NotFound("Cant find Claims");
      }
      var _role = await _roleManager.FindByIdAsync(claim.RoleId);
      if( _role == null )
      {
         return NotFound("Cant find Role");
      }
      Role = _role;
      Input = new InputModel
      {
         Type  = claim.ClaimType,
         Value = claim.ClaimValue
      };
      return Page();
   }
   public async Task<IActionResult> OnPost(int? claimId)
   {
      if( claimId == null )
      {
         return Page();
      }
      var claim = await _myBlogContext.RoleClaims.FirstOrDefaultAsync(x => x.Id == claimId);
      if( claim == null )
      {
         return Page();
      }
      var _role = await _roleManager.FindByIdAsync(claim.RoleId);
      if( _role == null )
      {
         return Page();
      }

      if( !ModelState.IsValid )
      {
         return Page();
      }
      
      var isAny = await _myBlogContext.RoleClaims.AnyAsync(x => x.RoleId == _role.Id &&
         x.ClaimType == Input.Type && x.ClaimValue == Input.Value && x.Id != claim.Id);
      if(isAny)
      {
         ModelState.AddModelError(string.Empty,$"Already have {Input.Type}");
         return Page();
      }
      claim.ClaimType = Input.Type;
      claim.ClaimValue = Input.Value;
      
      await _myBlogContext.SaveChangesAsync();

      StatusMessage = "Update Claim";
      return RedirectToPage("Index");
   }
}