using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorweb.models;

namespace razorweb.Areas.Admin.Pages.Role;
public class AddRoleClaimModel : RoleManagerBase
{

    public AddRoleClaimModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
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
    
    public async Task<IActionResult> OnGet(string roleId)
    {
        if (string.IsNullOrEmpty(roleId))
        {
            RedirectToPage("Index");
        }
        var _role = await _roleManager.FindByIdAsync(roleId);
        if (_role == null)
        {
            StatusMessage = "None Role";
            RedirectToPage("Index");
        }
        Role = _role;
        
        return Page();
    }
    public async Task<IActionResult> OnPost(string roleId)
    {
        if (string.IsNullOrEmpty(roleId))
        {
            RedirectToPage("Index");
        }
        var _role = await _roleManager.FindByIdAsync(roleId);
        if (_role == null)
        {
            StatusMessage = "None Role";
            RedirectToPage("Index");
        }
        var isAny = (await _roleManager.GetClaimsAsync(_role)).Any(x=>x.Type == Input.Type && x.Value == Input.Value);
        if( isAny )
        {
            ModelState.AddModelError(string.Empty, $"Already have {Input.Type}");
            return Page();
        }
        var newClaim = new Claim(Input.Type, Input.Value);
        var result = await _roleManager.AddClaimAsync(_role,newClaim);
        if( !result.Succeeded )
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        }
        return RedirectToPage("Index");
    }
}