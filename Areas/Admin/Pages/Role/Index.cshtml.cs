using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorweb.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace razorweb.Areas.Admin.Pages.Role
{
    public class IndexModel : RoleManagerBase
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }

        public List<IdentityRole> Roles { get; set; }

        public async Task OnGet()
        {

            Roles = await _roleManager.Roles.ToListAsync();
        }
        public void OnPost() => RedirectToPage();
    }
}
