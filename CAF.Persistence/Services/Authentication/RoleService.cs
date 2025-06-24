using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAF.Application.Abstractions.Services.Authentication;
using CAF.Application.Models;
using CAF.Application.Models.Authentication.DTOs;
using CAF.Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CAF.Persistence.Services.Authentication;

public class RoleService : IRoleService
{
    readonly RoleManager<AppRole> _roleManager;

    public RoleService(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> CreateRole(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty.", nameof(name));
        AppRole role = new AppRole
        {
            Name = name,
            Description = description
        };
        IdentityResult result = await _roleManager.CreateAsync(role);
        return result.Succeeded;
    }

    public async Task<bool> DeleteRole(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Role ID cannot be empty.", nameof(id));
        AppRole role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);
        if (role == null)
            throw new KeyNotFoundException($"Role with ID {id} not found.");
        IdentityResult result = await _roleManager.DeleteAsync(role);
        return result.Succeeded;
    }

    public Pagination<RoleDto> GetAllRoles(int page, int size)
    {
        if (page < 1 || size < 1)
            throw new ArgumentOutOfRangeException("Page and size must be greater than zero.");
        IQueryable<AppRole> rolesQuery = _roleManager.Roles;
        int totalCount = rolesQuery.Count();
        List<AppRole> roles = rolesQuery.Skip((page - 1) * size).Take(size).ToList();
        List<RoleDto> roleDtos = roles.Select(role => new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        }).ToList();
        return new Pagination<RoleDto>(roleDtos, totalCount, page, size);
    }

    public async Task<(string id, string name)> GetRoleById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Role ID cannot be empty.", nameof(id));
        AppRole role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if (role == null)
            throw new KeyNotFoundException($"Role with ID {id} not found.");
        return (role.Id, role.Name);
    }

    public async Task<bool> UpdateRole(string id, string name)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Role ID cannot be empty.", nameof(id));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty.", nameof(name));
        AppRole role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);
        if (role == null)
            throw new KeyNotFoundException($"Role with ID {id} not found.");
        role.Name = name;
        IdentityResult result = await _roleManager.UpdateAsync(role);
        return result.Succeeded;
    }
}
