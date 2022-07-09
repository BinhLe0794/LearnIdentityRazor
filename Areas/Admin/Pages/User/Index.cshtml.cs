using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using razorweb.models;

namespace razorweb.Areas.Admin.Pages.User;
[Authorize]
public class IndexModel : UserManagerBase
{
   public IndexModel(UserManager<AppUser> userManager, MyBlogContext myBlogContext) : base(userManager,
      myBlogContext)
   {
   }
   public List<UserAndRole> Users { get; set; } = new();
   public async Task OnGet()
   {
      var _users = await _userManager.Users.ToListAsync();
      foreach(var user in _users)
      {
         var userRecord = new UserAndRole
         {
            Id       = user.Id,
            UserName = user.UserName
         };
         var _roleArray = (await _userManager.GetRolesAsync(user)).ToArray();
         userRecord.Roles = string.Join(",", _roleArray);
         Users.Add(userRecord);
      }
   }
   public void OnPost()
   {
      RedirectToPage();
   }
   public class UserAndRole : AppUser
   {
      public string Roles { get; set; }
   }
}