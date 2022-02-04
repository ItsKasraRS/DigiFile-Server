using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BackEnd.Core.Utilities.Security
{
    public static class GetUserIdExtension
    {
        public static long GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                var result = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                return Convert.ToInt64(result.Value);
            }
            return default(long);
        }
    }
}
