using CAF.Application;
using CAF.Infrastructure;
using CAF.Persistence;
using CAF.WEB.Extensions.Exceptions;
using CAF.WEB.Extensions.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Added 

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CustomExceptionFilter>(); // Exception filter'ý ekleyin
    options.Filters.Add<DynamicAuthorizationAttribute>(); 

});

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Auth/Login";
    options.LogoutPath = "/Auth/Auth/Logout";
    options.AccessDeniedPath = "/Auth/Auth/AccessDenied";
    options.SlidingExpiration = true;
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

}
#region Added 

else
{
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors();
#endregion


app.UseAuthentication();
app.UseAuthorization();


//app.MapStaticAssets();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();

