using BooksStore.Infrastructure;
using BooksStore.Infrastructure.Authentication;
using BookStore.API.Extencions;
using BookStore.Application.Mappings;
using BookStore.Application.Services;
using BookStore.Core.Abstactions;
using BookStore.Core.Enums;
using BookStrore.DataAccess;
using BookStrore.DataAccess.Repositories;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.Configure<AuthorizationOptions>(builder.Configuration.GetSection(nameof(AuthorizationOptions)));

builder.Services.AddDbContext<BookStoreDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(BookStoreDbContext)));
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
builder.Services.AddApiAuthentication(Options.Create(jwtOptions));

builder.Services.AddAutoMapper(typeof(UserMappingProfile).Assembly);

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore API v1");
    c.RoutePrefix = "";
});


app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:3000");
    x.WithMethods().AllowAnyMethod();
});

app.MapGet("get", () =>
{
    return Results.Ok("Ok");
}).RequirePermissions(Permission.Read);

app.MapPost("post", () =>
{
    return Results.Ok("Ok");
}).RequirePermissions(Permission.Create);

app.MapPut("put", () =>
{
    return Results.Ok("Ok");
}).RequirePermissions(Permission.Update);

app.MapDelete("delete", () =>
{
    return Results.Ok("Ok");
}).RequirePermissions(Permission.Delete);

app.Run();
