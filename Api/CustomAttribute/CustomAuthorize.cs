
using Api.Common;
using Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Api.CustomAttribute
{

    public class CustomAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        private UserRoleType[] _roles;
        public CustomAuthorize(params UserRoleType[] role)
        {
            _roles = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var appDbContext = new Context();
            if (!user.Identity.IsAuthenticated)
                return;

            var userRoles = new List<long>();

            var getUserId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (getUserId != null)
            {
                userRoles = appDbContext.UserRoles.Include(x => x.Role).Where(x => x.UserId == Convert.ToInt64(getUserId)).Select(x => x.Role.Id).ToList();
            }

            if (_roles != null && _roles.Any(r => userRoles.Contains(Convert.ToInt64(r))) == false)
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                return;
            }

            var getToken = appDbContext.UserTokens.FirstOrDefault(x => x.UserId == Convert.ToInt64(getUserId) && x.IsDelete == false);
            if (getToken == null || getToken.Token == null)
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
                return;
            }
        }
    }



}
