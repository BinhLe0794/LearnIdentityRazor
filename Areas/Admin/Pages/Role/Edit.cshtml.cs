using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorweb.models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace razorweb.Areas.Admin.Pages.Role
{
    public class EditModel : RoleManagerBase
    {
        public EditModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }
        public class InputModel
        {
            [Required]
            [StringLength(10,MinimumLength =3, ErrorMessage ="{0} must {2} to {1}")]
            public string Name { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public async void OnGet(string roleId)
        {
            System.Console.WriteLine(roleId);
            if (string.IsNullOrEmpty(roleId))
            {
                 RedirectToPage("Index");
            }
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                StatusMessage = "None Role";
                RedirectToPage("Index");
            }
            Input = new InputModel()
            {
                Name = role.Name
            };

        }

        public async Task<IActionResult> OnPost(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                StatusMessage = "None Role";
                RedirectToPage("Index");
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                StatusMessage = "None Role";
                RedirectToPage("Index");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            role.Name = Input.Name;

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return Page();
                }
            }
            StatusMessage = $"Update Success {Input.Name} Role";
            return RedirectToPage("Index");
        }
    }
}
