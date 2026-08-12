using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using WarehouseERP.Api.DependencyInjection;
using WarehouseERP.Api.Middleware;
using WarehouseERP.Infrastructure.DependencyInjection;
using WarehouseERP.Infrastructure.Identity;

const string BlazorClientCorsPolicy = "BlazorClient";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Every controller action requires an authenticated caller by default (secure-by-default);
// a future public endpoint would need an explicit [AllowAnonymous].
builder.Services.AddControllers(options => options.Filters.Add(new AuthorizeFilter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddApplicationIdentity();
builder.Services.AddErpAuthorizationPolicies();
builder.Services.AddCors(options =>
{
    options.AddPolicy(BlazorClientCorsPolicy, policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5096",
                "https://localhost:7210")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors(BlazorClientCorsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var identity = app.MapGroup("/api/auth");
identity.MapIdentityApi<ApplicationUser>();

// Public self-registration is disabled for this internal ERP — every other Identity API
// endpoint (login, refresh, forgotPassword, manage/*, ...) stays mapped as-is.
identity.AddEndpointFilter(async (context, next) =>
{
    var request = context.HttpContext.Request;
    if (request.Method == HttpMethods.Post && request.Path == "/api/auth/register")
    {
        return Results.NotFound();
    }

    return await next(context);
});

// MapIdentityApi does not include a logout route — the cookie must be actively cleared.
// Requiring a JSON body (even an empty one) is a deliberate CSRF mitigation: a cross-site
// HTML form POST cannot send an application/json body without triggering a CORS preflight,
// which the origin allowlist above would then reject.
app.MapPost("/api/auth/logout", async (
    SignInManager<ApplicationUser> signInManager,
    [FromBody] object empty) =>
{
    if (empty is null)
    {
        return Results.Unauthorized();
    }

    await signInManager.SignOutAsync();
    return Results.Ok();
}).RequireAuthorization();

using (var scope = app.Services.CreateScope())
{
    var identitySeeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();
    await identitySeeder.SeedAsync();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
