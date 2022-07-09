using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using razorweb.models;

namespace razorweb.Pages;
public class IndexModel : PageModel
{
   private readonly ILogger<IndexModel> _logger;
   private readonly MyBlogContext myBlogContext;
   public IndexModel(ILogger<IndexModel> logger, MyBlogContext _myContext)
   {
      _logger       = logger;
      myBlogContext = _myContext;
   }
   public void OnGet()
   {
      var posts = (from a in myBlogContext.articles
                   orderby a.Created descending
                   select a).ToList();
      ViewData["posts"] = posts;
   }
}