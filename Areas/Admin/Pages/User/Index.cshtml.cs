using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorweb.Areas.Admin.Pages.User;
using razorweb.models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace razorweb.Areas.Admin.Pages.User
{
    [Authorize]
    public class IndexModel : UserManagerBase
    {
        public IndexModel(UserManager<AppUser> userManager, MyBlogContext myBlogContext) : base(userManager, myBlogContext)
        {
        }

        public List<UserAndRole> Users { get; set; } = new List<UserAndRole>();
        public class UserAndRole : AppUser
        {
            public string Roles { get; set; }
        }
        public async Task OnGet()
        {
            
            var _users = await _userManager.Users.ToListAsync();

            foreach (var user in _users)
            {
                var userRecord = new UserAndRole()
                {
                    Id = user.Id,
                    UserName = user.UserName
                };
                var _roleArray = (await _userManager.GetRolesAsync(user)).ToArray();

                userRecord.Roles = string.Join(",", _roleArray);
                Users.Add(userRecord);
            }
        }
        public void OnPost() => RedirectToPage();
    }
}
