using CAF.Application.Abstractions.Services.Authentication;
using CAF.Application.Models.Authentication.Requests;
using CAF.Application.Models.Authentication.VM;
using CAF.Application.Models.Common;
using CAF.Application.Repositories;
using CAF.Domain.Entities.Authentication;
using CAF.WEB.Extensions.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CAF.WEB.Areas.Management.Controllers;

[Authorize]
[Area("Management")]
public class UserManagerController : Controller
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public UserManagerController(IUserService userService, IRoleService roleService, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userService = userService;
        _roleService = roleService;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] PaginationRequest request)
    {
        var response = await _userService.GetAllUsersAsync(request.Page, request.Size);

        return View(response);
    }

    public async Task<IActionResult> AssignRoleToUser([FromQuery] PaginationRequest request)
    {

        var users = _userManager.Users.ToList();

        var model = new AssignRoleViewModel
        {
            Users = users.Select(x => new AppUser
            {
                Id = x.Id,
                UserName = x.UserName
            }).ToList(),

            //Roles = roles.Select(x => new AppRole
            //{
            //    Name = x.Name
            //}).ToList(),
        };



        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRoleToUser(AssignRoleToUserRequest model)
    {

        if (ModelState.IsValid)
        {

            await _userService.AssignRoleToUserAsnyc(model.UserId, model.Roles);

            TempData["SuccessMessage"] = "Rol başarılı bir şekilde atandı.";

            return Redirect(Request.Headers["Referer"].ToString());

        }

        TempData["Message"] = "Bir Problem Oluştu ";
        TempData["MessageType"] = "error";
        return Redirect(Request.Headers["Referer"].ToString());


    }

    [HttpGet("GetRolesToUser/{userId}")]
    public async Task<IActionResult> GetRolesToUser([FromRoute] string userId)
    {
        var response = await _userService.GetRolesToUserAsync(userId);
        return Ok(new { UserRoles = response });
    }

    [HttpGet("GetAllRoles")]
    public async Task<IActionResult> GetAllRoles([FromQuery] PaginationRequest request)
    {
        var response = _roleService.GetAllRoles(request.Page, request.Size);

        return Ok(response);
    }
}
