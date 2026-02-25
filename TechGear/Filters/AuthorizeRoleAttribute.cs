using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TechGear.Filters
{
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userRole = context.HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userRole))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            if (!_roles.Contains(userRole))
            {
                // Redirect to AccessDenied action with proper URL change
                context.Result = new RedirectToActionResult("AccessDenied", "Error", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
