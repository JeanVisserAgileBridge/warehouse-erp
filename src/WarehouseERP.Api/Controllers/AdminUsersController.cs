using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WarehouseERP.Api.Contracts.Auth;
using WarehouseERP.Api.DependencyInjection;
using WarehouseERP.Infrastructure.Identity;
using WarehouseERP.Shared.Contracts.Auth;

namespace WarehouseERP.Api.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Policy = PolicyNames.AdminOnly)]
public sealed class AdminUsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminUsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserSummaryResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.ToListAsync(cancellationToken);
        var responses = new List<UserSummaryResponse>(users.Count);

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            responses.Add(user.ToUserSummaryContract(roles));
        }

        return Ok(responses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserSummaryResponse>> GetById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(user.ToUserSummaryContract(roles));
    }

    [HttpPost]
    public async Task<ActionResult<UserSummaryResponse>> Create(CreateUserRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            return ValidationProblem(BuildModelState(createResult));
        }

        if (request.Roles.Count > 0)
        {
            var roleResult = await _userManager.AddToRolesAsync(user, request.Roles);

            if (!roleResult.Succeeded)
            {
                return ValidationProblem(BuildModelState(roleResult));
            }
        }

        var roles = await _userManager.GetRolesAsync(user);
        var contract = user.ToUserSummaryContract(roles);

        return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
    }

    [HttpPut("{id}/roles")]
    public async Task<ActionResult<UserSummaryResponse>> AssignRoles(string id, AssignRolesRequest request)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        var rolesToAdd = request.Roles.Except(currentRoles).ToArray();
        var rolesToRemove = currentRoles.Except(request.Roles).ToArray();

        // Add before remove: if the add step fails, the user still holds every role they had
        // before this call rather than being left with none.
        if (rolesToAdd.Length > 0)
        {
            var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);

            if (!addResult.Succeeded)
            {
                return ValidationProblem(BuildModelState(addResult));
            }
        }

        if (rolesToRemove.Length > 0)
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            if (!removeResult.Succeeded)
            {
                return ValidationProblem(BuildModelState(removeResult));
            }
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(user.ToUserSummaryContract(roles));
    }

    private static ModelStateDictionary BuildModelState(IdentityResult result)
    {
        var modelState = new ModelStateDictionary();

        foreach (var error in result.Errors)
        {
            modelState.AddModelError(error.Code, error.Description);
        }

        return modelState;
    }
}
