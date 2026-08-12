using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WarehouseERP.Infrastructure.Identity;

public sealed class IdentitySeeder
{
    private const string SeedEmailKey = "Identity:SeedUser:Email";
    private const string SeedPasswordKey = "Identity:SeedUser:Password";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleSeeder _roleSeeder;
    private readonly IConfiguration _configuration;
    private readonly ILogger<IdentitySeeder> _logger;

    public IdentitySeeder(
        UserManager<ApplicationUser> userManager,
        RoleSeeder roleSeeder,
        IConfiguration configuration,
        ILogger<IdentitySeeder> logger)
    {
        _userManager = userManager;
        _roleSeeder = roleSeeder;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await _roleSeeder.SeedAsync(cancellationToken);

        var email = _configuration[SeedEmailKey];
        var password = _configuration[SeedPasswordKey];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning(
                "Seed user not created — '{EmailKey}' / '{PasswordKey}' configuration is missing.",
                SeedEmailKey,
                SeedPasswordKey);
            return;
        }

        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Seed user could not be created: {Errors}", errors);
                return;
            }
        }

        if (!await _userManager.IsInRoleAsync(user, Roles.Admin))
        {
            await _userManager.AddToRoleAsync(user, Roles.Admin);
        }
    }
}
