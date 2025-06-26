using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using CAF.Application.Abstractions.Services.Permission;

namespace CAF.WEB.Extensions.Exceptions;

public class DynamicAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        var user = httpContext.User;

        if (!user.Identity.IsAuthenticated)
            return;

        var routeData = context.ActionDescriptor.RouteValues;
        var currentArea = routeData.TryGetValue("area", out var area) ? area : null;
        var currentController = routeData.TryGetValue("controller", out var controller) ? controller : null;
        var currentAction = routeData.TryGetValue("action", out var action) ? action : null;

        var menuService = httpContext.RequestServices.GetRequiredService<IMenuService>();
        var menus = await menuService.GetMenusAsync();

        var isMenuPage = menus.Any(menu => menu.ControllerName == currentController && menu.ActionName == currentAction);
        if (!isMenuPage)
            return;

        var navigationMenuService = httpContext.RequestServices.GetRequiredService<INavigationMenuService>();
        var hasPermission = await navigationMenuService.HasPermissionAsync(user, currentController, currentAction, currentArea);

        if (!hasPermission)
        {
            if (IsAjaxOrJsonRequest(httpContext.Request))
            {
                context.Result = new JsonResult(new { error = "Bu işlemi gerçekleştirmek için yetkiniz yok." })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
            else
            {
                context.Result = new ViewResult
                {
                    ViewName = "Error",
                    ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
                {
                    { "ErrorMessage", "Bu işlemi gerçekleştirmek için yetkiniz yok." }
                }
                };
            }
        }
    }

    private bool IsAjaxOrJsonRequest(HttpRequest request)
    {
        return request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
               request.Headers["Accept"].Any(x => x.Contains("application/json"));
    }

}
