using CAF.Application.Abstractions.Services.Permission;
using CAF.Application.Models.Permission.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace CAF.WEB.ViewComponents.ManagerLayout;

[ViewComponent(Name = "NavigationMenu")]
public class NavigationMenuComponent : ViewComponent
{
    private readonly INavigationMenuService _navigationMenuService;
    private readonly IDistributedCache _cache;

    public NavigationMenuComponent(INavigationMenuService navigationMenuService, IDistributedCache cache)
    {
        _navigationMenuService = navigationMenuService;
        _cache = cache;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = HttpContext.User; // Kullanıcı bilgilerini al
        var cacheKey = $"NavigationMenu_{user.Identity?.Name}";

        // Önce cache kontrolü yap
        var cachedMenu = await _cache.GetStringAsync(cacheKey);
        List<NavigationMenuViewModel> menuItems;

        if (!string.IsNullOrEmpty(cachedMenu))
        {
            // Cache'den menüleri getir
            menuItems = System.Text.Json.JsonSerializer.Deserialize<List<NavigationMenuViewModel>>(cachedMenu);
        }
        else
        {
            // Cache'de yoksa back-end'e gidip menüleri getir
            menuItems = await _navigationMenuService.GetMenuItemsAsync(user);

            // Cache'e ekle (10 dakika geçerli)
            var serializedMenu = System.Text.Json.JsonSerializer.Serialize(menuItems);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
            await _cache.SetStringAsync(cacheKey, serializedMenu, cacheOptions);
        }

        return View(menuItems);
    }
}