using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorweb.models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace razorweb.Areas.Admin.Pages.Role
{
    public class IndexModel : RoleManagerBase
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }
        public class RoleModel : IdentityRole
        {
            public string[] Claims { get; set; } 
        }
        public List<RoleModel> Roles { get; set; } = new List<RoleModel>();

        public async Task OnGet()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            foreach(var role in roles)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                var claimArray = claims.Select(x => x.Type + " " + x.Value);
                var roleVm = new RoleModel()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Claims = claimArray.ToArray()
                };
                Roles.Add(roleVm);
            }
        }
        public void OnPost() => RedirectToPage();
    }
}
