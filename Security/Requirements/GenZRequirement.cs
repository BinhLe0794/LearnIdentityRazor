using Microsoft.AspNetCore.Authorization;

namespace razorweb.Security.Requirements;
//kế thừa
public class GenZRequirement : IAuthorizationRequirement
{
   public GenZRequirement(int fromYear = 1997, int toYear = 2012)
   {
      FromYear = fromYear;
      ToYear   = toYear;
   }
   public int FromYear { get; set; }
   public int ToYear { get; set; }
}