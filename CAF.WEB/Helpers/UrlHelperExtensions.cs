using Microsoft.AspNetCore.Mvc;

namespace CAF.WEB.Helpers;

public static class UrlHelperExtensions
{
    public static bool IsMenuActive(this IUrlHelper urlHelper, string controller, string action = null, string area = null)
    {
        var routeData = urlHelper.ActionContext.RouteData.Values;
        bool isActive = routeData["controller"]?.ToString() == controller;

        if (!string.IsNullOrEmpty(action))
        {
            isActive = isActive && routeData["action"]?.ToString() == action;
        }

        if (!string.IsNullOrEmpty(area))
        {
            isActive = isActive && routeData["area"]?.ToString() == area;
        }

        return isActive;
    }

    public static bool IsMenuActive(this IUrlHelper urlHelper, params (string controller, string action, string area)[] routes)
    {
        var currentController = urlHelper.ActionContext.RouteData.Values["controller"]?.ToString();
        var currentAction = urlHelper.ActionContext.RouteData.Values["action"]?.ToString();
        var currentArea = urlHelper.ActionContext.RouteData.Values["area"]?.ToString();

        return routes.Any(route =>
            string.Equals(route.controller, currentController, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(route.action, currentAction, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(route.area, currentArea, StringComparison.OrdinalIgnoreCase));
    }
}
