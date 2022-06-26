using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorweb.models;

namespace razorweb.Areas.Admin.Pages.User
{
    public class UserManagerBase : PageModel
    {
        protected readonly UserManager<AppUser> _userManager;
        protected readonly MyBlogContext _myBlogContext;
        [TempData]
        public string StatusMessage { get; set; }
        public UserManagerBase(UserManager<AppUser> userManager, MyBlogContext myBlogContext)
        {
            _userManager = userManager;

            _myBlogContext = myBlogContext;
        }
    }
}
